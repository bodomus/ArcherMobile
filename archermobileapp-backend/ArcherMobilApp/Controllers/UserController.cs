using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

using AutoMapper;
using NToastNotify;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

using Archer.AMA.BLL.Contracts;
using ArcherMobilApp.Controllers.Base;
using ArcherMobilApp.DAL.MsSql.Contract;
using ArcherMobilApp.DAL.MsSql.Models;
using ArcherMobilApp.Infrastracture;
using ArcherMobilApp.Infrastracture.Extensions;
using ArcherMobilApp.Models;
using ArcherMobilApp.Models.AuxiliaryModels;
using ArcherMobilApp.Models.Swagger;
using Archer.AMA.DTO;
using ArcherMobilApp.BLL.Models;

namespace ArcherMobilApp.Controllers
{

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class UserController : EditableWebApiController<UserDTO, string, IUserService>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserController(IConfiguration configuration, IMapper mapper, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, IUserRepository userRepository, IUserService service) : base(service)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Return user</returns>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UserDTO), Description = "Return user")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UserModelExample))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid user id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult<UserDTO>> Get(string id)
        {
            return Ok(await base.GetById(id));
        }


        /// <summary>
        /// Get current user
        /// </summary>
        /// <returns>Return user</returns>
        [HttpGet("Current")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(string), Description = "Return user")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid user")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins, Employees")]
        public async Task<ActionResult<UserDTO>> GetCurrent()
        {
            var result = await Service.GetCurrentAsync(HttpContext.User?.Identity?.Name);
            return Ok(result);
        }

        /// <summary>
        /// Confirms ICR flag for current user.
        /// </summary>
        /// <returns>Return user</returns>
        [HttpPut("Current/ICRConfirm")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(string), Description = "Return OK")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins, Employees")]
        public async Task<ActionResult> ConfirmICR()
        {
            await Service.ConfirmICR(HttpContext.User?.Identity?.Name);
            return Ok();
        }

        /// <summary>
        /// Reset ICR confirmation flag for all users.
        /// </summary>
        /// <returns></returns>
        [HttpPut("ICRReset")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Return OK")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> ResetICR()
        {
            await Service.ResetICR(HttpContext.User?.Identity?.Name);
            return Ok();
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Return users</returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<UserDTO>), Description = "Return users")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UserModelExample))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid user id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            return Ok(await base.All());
        }

        /// <summary>
        /// Update existing user from mobile app
        /// </summary>
        /// <param name="user">User for update</param>
        /// <returns></returns>
        [HttpPut("Mobile/Update")]
        [SwaggerRequestExample(typeof(UserMobile), typeof(UserMobile))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid User object")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody]UserMobile user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var userInDb = await _userManager.FindByIdAsync(user.Id);
            if (userInDb == null)
                return NotFound($"User with Id {user.Id} not found.");

            if (string.Compare(user.NewEmail, userInDb.Email) != 0)
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(userInDb, user.NewEmail);
                await _userManager.ChangeEmailAsync(userInDb, user.NewEmail, token);
            }
            Log.Information($"Update raised: {user.UserName}");
            
            if (!string.IsNullOrEmpty(user.NewPassword))
            {
                var resultCheckPass = await _userManager.CheckPasswordAsync(userInDb, user.OldPassword);
                if (!resultCheckPass)
                {
                    return BadRequest("Old password does not valid");
                }

                var passSuccessfulAsync = await _userManager.ChangePasswordAsync(userInDb, user.OldPassword, user.NewPassword);
                if (!passSuccessfulAsync.Succeeded)
                {
                    return BadRequest("Cannot change password");
                }
            }

            await Service.UpdateMobilUser(user.UserName, user.NewEmail, HttpContext.User?.Identity?.Name);
            return Ok();
        }

        /// <summary>
        /// Update existing user
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [SwaggerRequestExample(typeof(UserWithPasswordDTO), typeof(UserWithPasswordDTO))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid User object")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<ActionResult> Put([FromBody]UserWithPasswordDTO model)
        {
            try
            {
                var userInDb = await _userManager.FindByIdAsync(model.Id);
                if (userInDb == null)
                    return NotFound($"User with Id {model.Id} not found.");

            var token = await _userManager.GenerateChangeEmailTokenAsync(userInDb, model.Email);
            await _userManager.ChangeEmailAsync(userInDb, model.Email, token);

                var existsRoles = await _userManager.GetRolesAsync(userInDb);
                await _userManager.RemoveFromRolesAsync(userInDb, existsRoles);
                await _userManager.AddToRoleAsync(userInDb, model.InputRole);

                await Update(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }


        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="dto">User DTO</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(UserDTO), typeof(UserDTO))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid User object")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(UserDTO), Description = "Returns created user")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult<UserDTO>> Post([FromBody]UserWithPasswordDTO dto)
        {

            var existRole = (await _userRepository.GetRolesAsync()).Where(a => string.CompareOrdinal(a.Name.ToLower(), dto.InputRole.ToLower()) == 0).SingleOrDefault();
            if (existRole == null)
                return BadRequest($"Role '{dto.InputRole}' doesn't exist in db.");

            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                try
                {
                    await _userManager.AddToRoleAsync(user, existRole.Name);
                    dto.Id = user.Id;
                    return await Create(dto);
                }
                catch (Exception ex)
                {
                    return Conflict(ex);
                }
            }
            else
            {
                return BadRequest(string.Join(",", result.Errors.Select(obj => $"{obj.Code}: {obj.Description}")));
            }
        }

        [HttpDelete("{id}")]
        [SwaggerRequestExample(typeof(UserEntity), typeof(UserEntity))]
        [SwaggerResponse((int)HttpStatusCode.NoContent, Description = "Returns 204")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid User object")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Remove(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                if (await _userManager.IsInRoleAsync(user, "admins"))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, @"Can not remove. User is in admin role.");
                }

                if (await base.Delete(id))
                {
                    await _userManager.DeleteAsync(user);
                    return NoContent();
                }
                
            }
            else if (user == null)
            {
                return NotFound("User not found");
            }

            return BadRequest("Something wrong");
        }


        [HttpPost("Lockout")]
        [SwaggerRequestExample(typeof(SimpleRequest), typeof(SimpleRequest))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid User object")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<ActionResult> Lockout([FromBody]SimpleRequest model)
        {
            var user = await _userManager.FindByIdAsync(model.Key);
            if (user != null)
            {
                bool flag = Convert.ToBoolean(model.Value);
                DateTimeOffset? date = flag ? (DateTimeOffset?)DateTime.Now.AddYears(10) : null;
                await _userManager.SetLockoutEnabledAsync(user, flag);
                await _userManager.SetLockoutEndDateAsync(user, date);
                string status = flag ? "locked" : "unlocked";

                return Ok();
            }
            else if (user == null)
            {
                return NotFound("User not found");
            }

            return BadRequest("Something wrong");
        }

        /// <summary>
        /// Authentication and return token and  token  refresh if successfully.
        /// </summary>
        /// <param name="model">LoginViewModel model</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(LoginResponse), Description = "Return token for user")]
        [SwaggerResponse((int)HttpStatusCode.Forbidden, Description = "User account locked out.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Invalid login attempt.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerRequestExample(typeof(LoginViewModel), typeof(LoginModelExample))]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    Log.Information("User logged in.");
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var isLocked = await _userManager.IsLockedOutAsync(user);
                    if (isLocked)
                        return new ForbidResult("User is locked out.");

                    var role = await _userManager.GetRolesAsync(user);
                    var token = new TokenGenerator(_configuration).GenerateToken(model.Email, role);

                    var claim = HttpContext?.User?.Claims?.Where(obj => obj.Type == JwtRegisteredClaimNames.Jti).FirstOrDefault();

                    await _userRepository.RemoveRefreshToken(user.Id);
                    await _userRepository.AddRefreshToken(user.Id, token.Jti);

                    await _userRepository.UpdateLastUserLoginAsync(user.Id, DateTime.Now);
                    return Ok(new LoginResponse() { AccessToken = token.Value, Id = user.Id });
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    Log.Information("User account locked out.");

                    return StatusCode(403);
                }
                else
                {
                    return BadRequest("Invalid login attempt.");
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToPage("Login");
        }

        /// <summary>
        /// Logout current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(JsonResult), Description = "Logout successful.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("GetPagination")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerRequestExample(typeof(AgGridModel), typeof(AgGridParameterRequestModelExample))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "An error occured while update")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> GetUsers([FromQuery] AgGridModel dtParameters)
        {
            List<ComplexUserEntity> result = (_userRepository.GetUsersAsync().Result);

            var filteredResultsCount = result.Count();
            var take = dtParameters.EndRow - dtParameters.StartRow + 1;

            return new JsonResult(new
            {
                draw = dtParameters,
                recordsTotal = result.Count,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.StartRow)
                    .Take(take)
                    .ToList()
            });

            return Ok();
        }
    }
}