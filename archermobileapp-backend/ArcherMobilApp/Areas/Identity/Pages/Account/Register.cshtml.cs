using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

using NToastNotify;

using ArcherMobilApp.DAL.MsSql.Contract;
using ArcherMobilApp.DAL.MsSql.Models;
using ArcherMobilApp.Infrastracture.Extensions;

namespace ArcherMobilApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUserRepository _userRepository;
        private readonly IToastNotification _toastNotification;
        private readonly int _currentUser;
        private readonly IConfiguration _configuration;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IUserRepository userRepository, 
            IToastNotification toastNotification,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration
            )
        {
            _userRepository = userRepository;
            _toastNotification = toastNotification;

            _userManager = userManager;

            // Remove the restriction on only alphanumeric names 
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _currentUser = httpContextAccessor.CurrentUser();
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [Display(Name = "Roles")]
        public SelectList Roles { get; set; }
        public ComplexUserEntity Caption { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Roles")]
            public string InputRole { get; set; }

            [Display(Name = "Confirm ICR")]
            public bool ConfirmICR  { get; set; }

            [Display(Name = "Is admin")]
            public bool IsAdmin { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var list = (await _userRepository.GetRolesAsync()).ToList();
            Roles = new SelectList(list, "Id", "Name");
            Caption = new ComplexUserEntity();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email};
                var pass = _configuration.GetSection("SecretsApp:EmployesPassword").Value;

                var result = await _userManager.CreateAsync(user, pass);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var existRole = (await _userRepository.GetRolesAsync()).Where(a=> string.CompareOrdinal(a.Id, Input.InputRole) == 0).SingleOrDefault();
                    if (existRole == null)
                    {
                        _toastNotification.AddErrorToastMessage($"Role does''t exist if db.");
                        return Page();
                    }

                    var role = await _userManager.AddToRoleAsync(user, existRole.Name);
                    await _userRepository.CreateUserAsync(user.Id, new UserEntity()
                    {
                        IsAdmin = false,
                        Id = user.Id,
                        ConfirmICR = false,
                        UserName = Input.Name,
                        KeepLogged = false,
                        Email = Input.Email
                    });

                    _toastNotification.AddSuccessToastMessage($"Your successfully create user with params:\n Email:{user.Email}\n" +
                                                              $"Role: {existRole.Name}\n UserName: {user.UserName} \n ConfirmICR: {false}\n");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
