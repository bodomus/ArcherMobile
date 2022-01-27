using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ArcherMobilApp.DAL.MsSql.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NToastNotify;

namespace ArcherMobilApp.Areas.Identity.Pages.Account
{
    public class UsersModel : PageModel
    {
        private readonly ILogger<UsersModel> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IToastNotification _toastNotification;

        public UsersModel(
            ILogger<RegisterModel> logger,
            IUserRepository userRepository,
            IToastNotification toastNotification
        )
        {
            _userRepository = userRepository;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public UsersModel.InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            public List<ArcherMobilApp.BLL.Models.User> Users { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }
    }
}