using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

using NToastNotify;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using AutoMapper;

using ArcherMobilApp.BLL.Models;
using ArcherMobilApp.DAL.MsSql.Contract;
using ArcherMobilApp.DAL.MsSql.Models;
using ArcherMobilApp.Models.Swagger;
using ArcherMobilApp.Controllers.Base;
using Archer.AMA.BLL.Contracts;
using Archer.AMA.DTO;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ArcherMobilApp.Controllers
{
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    //[ApiVersion("1.0")]
    //[ApiController]
    
    //TODO Add base controller
    public class UsersController : EditableWebApiController<UserDTO, string, IUserService>
    {
        private readonly IUserRepository _userRepository;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _service;

        public UsersController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IUserRepository userRepository,
            IToastNotification toastNotification, IMapper mapper, IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor, IUserService service) : base(service)
        {
            _signInManager = signInManager;
            _userRepository = userRepository;
            _toastNotification = toastNotification;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Logout current user
        /// </summary>
        /// <returns></returns>
        //[HttpGet("logout")]
        //[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(JsonResult), Description = "Logout successful.")]
        //[SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        //public async Task<ActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    return Ok("Logout successful.");
        //}

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Return user</returns>
        //[HttpGet("{id}")]
        //[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UserEntity), Description = "Return user")]
        //[SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UserModelExample))]
        //[SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid user id")]
        //[SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        //[SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        //public async Task<ActionResult<UserEntity>> Get(string id)
        //{
        //    return await GetById(id);
        //}

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="id">User email</param>
        /// <returns>Return user</returns>
        [HttpGet("getUserByEmail/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(User), Description = "Return user")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UserModelExample))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid user id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<ActionResult<User>> GetUserByEmail(string id)
        {

            if (string.CompareOrdinal(User.Identity.Name, id) != 0)
                return BadRequest($"Current user does not  privilege for  view  information about {id}");

            var users = await _userRepository.GetUserByEmailAsync(id);
            if (users != null)
            {
                var d = await _userRepository.GetDocuments();
                List<Document> docs = new List<Document>();
                foreach (var documentEntity in d)
                {
                    docs.Add(new Document()
                    {
                        Id = documentEntity.Id,
                        Description = documentEntity.Description,
                        DocumentTypeId = documentEntity.DocumentTypeId,
                        Title = documentEntity.Title,
                        Uri = documentEntity.Uri
                    });
                }

                //TODO To Mapper interface
                User user = new User()
                {
                    UserName = users.UserName,
                    Email = users.Email,
                    ConfirmICR = users.ConfirmICR,
                    Id = users.Id
                };

                _toastNotification.AddSuccessToastMessage($"Your successfully get user with params:\n Email:{user.Email}\n" +
                                                          $"Role: {user.RoleName}\n UserName: {user.UserName} \n ConfirmICR: {user.ConfirmICR}\n");

                return BadRequest();
            }
            return NotFound();
        }



        /// <summary>
        /// Authentication and return token and  token  refresh if successfully.
        /// </summary>
        /// <param name="model">LoginViewModel model</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        //[HttpPost("Login")]
        //[AllowAnonymous]
        //[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(LoginResponse), Description = "Return token for user")]
        //[SwaggerResponse((int)HttpStatusCode.Forbidden, Description = "User account locked out.")]
        //[SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Invalid login attempt.")]
        //[SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        //[SwaggerRequestExample(typeof(LoginViewModel), typeof(LoginModelExample))]
        //public async Task<IActionResult> Login([FromBody]LoginViewModel model, string returnUrl)
        //{
        //    returnUrl = returnUrl ?? Url.Content("~/");

        //     if (ModelState.IsValid)
        //    {

        //        // This doesn't count login failures towards account lockout
        //        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        //        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        //        if (result.Succeeded)
        //        {
        //            Log.Information("User logged in.");
        //            var user = await _userManager.FindByEmailAsync(model.Email);
        //            var isLocked = await _userManager.IsLockedOutAsync(user);
        //            if (isLocked)
        //                return new ForbidResult("User is locked out.");

        //            var role = await _userManager.GetRolesAsync(user);
        //            var token = new TokenGenerator(_configuration).GenerateToken(model.Email);

        //            var claim = HttpContext?.User?.Claims?.Where(obj => obj.Type == JwtRegisteredClaimNames.Jti).FirstOrDefault();


        //            await _userRepository.RemoveRefreshToken(user.Id);
        //            await _userRepository.AddRefreshToken(user.Id, token.Jti);

        //            await _userRepository.UpdateLastUserLoginAsync(user.Id, DateTime.Now);
        //            return Ok( new LoginResponse() { AccessToken = token.Value, Id = user.Id });
        //        }
        //        if (result.RequiresTwoFactor)
        //        {
        //            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        //        }
        //        if (result.IsLockedOut)
        //        {
        //            Log.Information("User account locked out.");

        //            return StatusCode(403);
        //        }
        //        else
        //        {
        //            return BadRequest("Invalid login attempt.");
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return RedirectToPage("Login");
        //}

        /// <summary>
        /// Get new token and refresh token if old one is expired.
        /// </summary>
        /// <returns></returns>
        //[HttpPost("Refresh")]
        //[AllowAnonymous]
        //[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(LoginResponse), Description = "Get new token and refresh token if old one is expired.")]
        //[SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Invalid refresh token.")]
        //[SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        //public async Task<IActionResult> Refresh()
        //{
        //    var systemToken = new TokenGenerator(_configuration);

        //    var principal = systemToken.GetPrincipalFromExpiredToken(HttpContext.Request.Headers["Authorization"]);
        //    var username = principal.Identity.Name;
        //    var savedRefreshToken = await _userRepository.GetRefreshToken(username);

        //    var tokenJtiExists = principal.Claims?.Where(obj => obj.Type == JwtRegisteredClaimNames.Jti && obj.Value == savedRefreshToken.Item2).Any();

        //    if (!tokenJtiExists.HasValue || !tokenJtiExists.Value)
        //        return NotFound("Invalid refresh token");

        //    var newToken = systemToken.GenerateToken(principal.Identity.Name);

        //    await _userRepository.RemoveRefreshToken(savedRefreshToken.Item1);
        //    await _userRepository.AddRefreshToken(savedRefreshToken.Item1, newToken.Jti);

        //    await _userRepository.UpdateLastUserLoginAsync(savedRefreshToken.Item1, DateTime.Now);
        //    return Ok(new LoginResponse { Id = savedRefreshToken.Item1, AccessToken = newToken.Value});
        //}

        /// <summary>
        /// Update existing user
        /// </summary>
        /// <param name="user">User for update</param>
        /// <returns></returns>
        [HttpPost("update")]
        [SwaggerRequestExample(typeof(CreateUserMobile), typeof(CreateUserMobileExample))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid User object")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> Update(CreateUserMobile user)
        {
            Log.Information($"Update raised: {user.UserName}");
            var model = _mapper.Map<UserEntity>(user);//var model = _mapper.Map<User>(users);

            if (user == null)
            {
                return BadRequest();
            }

            //TODO return to Identity validation
            var userInDb = await _userManager.FindByEmailAsync(user.Email);

            
            if (!string.IsNullOrEmpty(user.NewPassword))
            {
                var resultCheckPass = await _userManager.CheckPasswordAsync(userInDb, user.OldPassword);
                if (!resultCheckPass)
                {
                    return BadRequest("Old password does not valid");
                }
                 
                //TODO Add password validators
                var passSuccessfulAsync = await _userManager.ChangePasswordAsync(userInDb, user.OldPassword, user.NewPassword);
                if (!passSuccessfulAsync.Succeeded)
                {
                    return BadRequest("Cannot change password");
                }
            }

            model.Id = userInDb.Id;
            
            var result = await _userRepository.UpdateUserAsync(model);
            return Ok(user);
        }


        /// <summary>
        /// Update status Inner Company Rule flag.
        /// </summary>
        /// <param name="model">SimpleRequest model</param>
        /// <returns></returns>
        //[HttpPost("setIcrFlag")]
        //[IgnoreAntiforgeryToken]
        //[SwaggerRequestExample(typeof(SimpleRequest), typeof(SetFlagRequestModelExample))]
        //[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(OkResult), Description = "Return status operation - 200")]
        //[SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        //public async Task<IActionResult> SetICRFlag([FromBody]SimpleRequest model)
        //{
        //    if (model == null)
        //        return BadRequest("Wrong request. Model is null.");

        //    var user = await _userManager.FindByEmailAsync(model.Key);
        //    if (user ==  null)
        //        return BadRequest("User not found.");

        //    await _userRepository.UpdateStatusICR(user.Id, Convert.ToBoolean(model.Value));
        //    return Ok();
        //}

        /// <summary>
        /// Update status keep logged flag.
        /// </summary>
        /// <param name="model">SimpleRequest model</param>
        /// <returns></returns>
        //[HttpPut("setKeepLoggedFlag")]
        //[IgnoreAntiforgeryToken]
        ////[SwaggerRequestExample(typeof(SimpleRequest), typeof(SetFlagRequestModelExample))]
        //[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(OkResult), Description = "Return status operation - 200")]
        //[SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        //public async Task<IActionResult> SetKeepLoggedFlag([FromBody]SimpleRequest model)
        //{
        //    if (model == null)
        //        return BadRequest("Wrong request. Model is null.");

        //    var user = await _userManager.FindByEmailAsync(model.Key);
        //    if (user == null)
        //        return BadRequest("User not found.");

        //    await _userRepository.UpdateKeepLoggedFlag(user.Id, Convert.ToBoolean(model.Value));
        //    return Ok();
        //}


        //[HttpPut]
        //public async Task<ActionResult> EditUser(EditUserViewModel model)
        //{
        //    if (model == null)
        //    {
        //        return BadRequest();
        //    }
        //    var userInDb = await _userManager.FindByEmailAsync(model.OldEmail);
        //    if (userInDb == null)
        //        return NotFound($"User with email {model.OldEmail} not found.");

        //    if (string.CompareOrdinal(model.OldEmail, model.Email) != 0)
        //    {
        //        var token = await _userManager.GenerateChangeEmailTokenAsync(userInDb, model.Email);
        //        var result = await _userManager.ChangeEmailAsync(userInDb, model.Email, token);
        //    }

        //    var roles = await _userRepository.GetRolesAsync();
        //    var role = roles.SingleOrDefault(a => string.CompareOrdinal(a.Id, model.InputRole) == 0).Name;
        //    var existsRoles = await _userManager.GetRolesAsync(userInDb);
        //    await _userManager.RemoveFromRolesAsync(userInDb, existsRoles);
        //    await _userManager.AddToRoleAsync(userInDb, role);
        //    UserEntity ue = new UserEntity()
        //    {
        //        Id = userInDb.Id,
        //        ConfirmICR = model.ConfirmICR,
        //        Email = model.Email,
        //        IsAdmin = model.IsAdmin,
        //        KeepLogged = false,
        //        UserName = model.Name
        //    };

        //    var updateStatus = await _userRepository.UpdateUserAsync(ue);
        //    return new JsonResult(new { statusCode = 200 });
        //}
    }
}
