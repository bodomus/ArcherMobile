using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

using ArcherMobilApp.Areas.Identity.Pages.Account;
using ArcherMobilApp.DAL.MsSql.Contract;
using ArcherMobilApp.Infrastracture;
using ArcherMobilApp.Models;
using ArcherMobilApp.Models.Swagger;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;

namespace ArcherMobilApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountController(IConfiguration configuration, IMapper mapper, 
            SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            IUserRepository userRepository
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
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
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(LoginModelResponseExample))]
        [SwaggerRequestExample(typeof(LoginViewModel), typeof(LoginModelExample))]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (!ModelState.IsValid)
            {
                var problemDetails = new ValidationProblemDetails(ModelState);

                return new ObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json" },
                    StatusCode = 403,
                };
            }
            
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
                    return NotFound("Invalid login attempt.");
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToPage("Login");
        }

        /// <summary>
        /// Get new token and refresh token if old one is expired.
        /// </summary>
        /// <returns></returns>
        [HttpPost("Refresh")]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(LoginResponse), Description = "Get new token and refresh token if old one is expired.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Invalid refresh token.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(LoginModelResponseExample))]
        public async Task<IActionResult> Refresh()
        {
            var systemToken = new TokenGenerator(_configuration);

            var principal = systemToken.GetPrincipalFromExpiredToken(HttpContext.Request.Headers["Authorization"]);
            var username = principal.Identity.Name;
            var savedRefreshToken = await _userRepository.GetRefreshToken(username);

            var tokenJtiExists = principal.Claims?.Where(obj => obj.Type == JwtRegisteredClaimNames.Jti && obj.Value == savedRefreshToken.Item2).Any();

            if (!tokenJtiExists.HasValue || !tokenJtiExists.Value)
                return NotFound("Invalid refresh token");

            var user = await _userManager.FindByEmailAsync(username);
            var role = await _userManager.GetRolesAsync(user);

            var newToken = systemToken.GenerateToken(principal.Identity.Name, role);

            await _userRepository.RemoveRefreshToken(savedRefreshToken.Item1);
            await _userRepository.AddRefreshToken(savedRefreshToken.Item1, newToken.Jti);

            await _userRepository.UpdateLastUserLoginAsync(savedRefreshToken.Item1, DateTime.Now);
            return Ok(new LoginResponse { Id = savedRefreshToken.Item1, AccessToken = newToken.Value });
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
            return Ok("Logout successfull.");
        }
    }

}