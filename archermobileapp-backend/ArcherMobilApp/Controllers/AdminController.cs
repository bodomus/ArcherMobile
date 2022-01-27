using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

using Serilog;
using NToastNotify;

using ArcherMobilApp.DAL.MsSql.Contract;
using ArcherMobilApp.DAL.MsSql.Models;
using ArcherMobilApp.Infrastracture.Extensions;
using ArcherMobilApp.Models;
using ArcherMobilApp.Models.AuxiliaryModels;

namespace ArcherMobilApp.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IToastNotification _toastNotification;
        private readonly IConfiguration _configuration;

        public AdminController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor,
            SignInManager<IdentityUser> signInManager, IToastNotification toastNotification, UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _configuration = configuration;
        }

        
        [HttpPost]
        public ActionResult GetUsers(DtParameters dtParameters)
        {
            var searchBy = dtParameters?.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }
            List<ComplexUserEntity> result = (_userRepository.GetUsersAsync().Result);
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.UserName != null && r.UserName.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.Email != null && r.Email.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.RoleName != null && r.RoleName.ToUpper().Contains(searchBy.ToUpper())) 
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            var filteredResultsCount = result.Count();
            var totalResultsCount = result.Count;

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }

        [HttpPost]
        public async Task<ActionResult> Lockout([FromBody]SimpleRequest model)
        {
            var user = await _userManager.FindByIdAsync(model.Key);
            if (user != null)
            {
                bool flag = Convert.ToBoolean(model.Value);
                DateTimeOffset? date = flag ? (DateTimeOffset?) DateTime.Now.AddYears(10) : null;
                await _userManager.SetLockoutEnabledAsync(user, flag);
                await _userManager.SetLockoutEndDateAsync(user, date);
                string status = flag ? "locked": "unlocked";

                _toastNotification.AddSuccessToastMessage($"User {user.UserName} was successful {status}.");
                return Json(new { statusCode = 200});
            }
            else if (user == null)
            {

                _toastNotification.AddAlertToastMessage($"User {model.Key} does not exist.");
                return NotFound("User not found");
            }

            return BadRequest("Something wrong");
        }


        // DELETE: api/Admin/5
        [HttpPost]
        public async Task<ActionResult> DeleteUser([FromBody] SimpleRequest model)
        {
            var user = await _userManager.FindByIdAsync(model.Key);
            
            
            if (user != null)
            {
                if (await _userManager.IsInRoleAsync(user, "admins"))
                {
                    return StatusCode(403);
                }

                await _userRepository.DeleteUserAsync(model.Key);
                await _userManager.DeleteAsync(user);
                _toastNotification.AddSuccessToastMessage($"User {user.UserName} was successful deleted.");
                return Json(new { statusCode = 200 });
            }
            else if (user == null)
            {

                _toastNotification.AddAlertToastMessage($"User {model.Key} does not exist.");
                return NotFound("User not found");
            }

            return BadRequest("Something wrong");
        }

        [HttpPost]
        public async Task<ActionResult> AddUser( AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                

                var pass = _configuration.GetSection("SecretsApp:EmployesPassword").Value;
                if (model.IsAdmin)
                {
                    pass = model.Password;
//                    _userManager.PasswordValidators.
//                    //TODO Add password validators
//                    var passSuccessfulAsync = await _userManager.ChangePasswordAsync(userInDb, user.OldPassword, user.NewPassword);
//                    if (!passSuccessfulAsync.Succeeded)
//                    {
//                        return BadRequest("Cannot change password");
//                    }
                    if (string.CompareOrdinal(model.ConfirmPassword, model.Password) != 0)
                    {
                        _toastNotification.AddErrorToastMessage($"confirm password does not match password.");
                        return BadRequest();
                    }
                }

                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, pass);

                if (result.Succeeded)
                {
                    Log.Information("User created a new account with password.");

                    var existRole = (await _userRepository.GetRolesAsync()).Where(a => string.CompareOrdinal(a.Id, model.InputRole) == 0).SingleOrDefault();
                    if (existRole == null)
                    {
                        _toastNotification.AddErrorToastMessage($"Role does''t exist if db.");
                        return BadRequest();
                    }

                    var role = await _userManager.AddToRoleAsync(user, existRole.Name);
                    await _userRepository.CreateUserAsync(user.Id, new UserEntity()
                    {
                        IsAdmin = model.IsAdmin,
                        Id = user.Id,
                        ConfirmICR = false,
                        UserName = model.Name,
                        KeepLogged = false,
                        Email = model.Email
                    });

                    _toastNotification.AddSuccessToastMessage($"Your successfully create user with params:\n Email:{user.Email}\n" +
                                                              $"Role: {existRole.Name}\n UserName: {user.UserName} \n ConfirmICR: {false}\n");
                    return new JsonResult(new { statusCode = 200 });

                }
                StringBuilder b = new StringBuilder();

                foreach (var error in result.Errors)
                {
                    b.AppendFormat($"{error.Description}");
                    b.AppendLine();
                }
                _toastNotification.AddErrorToastMessage(b.ToString());
            }
            else
            {
                if (ModelState.ErrorCount > 0)
                {
                    StringBuilder b = new StringBuilder();
                    foreach (var error in ModelState.Values)
                    {
                        foreach (ModelError error1 in error.Errors)
                        {
                            b.AppendFormat($"{error1.ErrorMessage}");
                            b.AppendLine();
                        }
                    }
                    _toastNotification.AddErrorToastMessage(b.ToString());
                    return BadRequest();
                }

            }

            return new JsonResult(new { statusCode = 200});
        }

        [HttpPost]
        public async Task<ActionResult> EditUser(EditUserViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var userInDb = await _userManager.FindByEmailAsync(model.OldEmail);
            if (userInDb == null)
                return NotFound($"User with email {model.OldEmail} not found.");

            if (string.CompareOrdinal(model.OldEmail, model.Email) != 0)
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(userInDb, model.Email);
                var result = await _userManager.ChangeEmailAsync(userInDb, model.Email, token);
            }

            var roles = await _userRepository.GetRolesAsync();
            var role =  roles.SingleOrDefault(a => string.CompareOrdinal(a.Id, model.InputRole) == 0).Name;
            var existsRoles = await _userManager.GetRolesAsync(userInDb);
            await _userManager.RemoveFromRolesAsync(userInDb, existsRoles);
            await _userManager.AddToRoleAsync(userInDb, role);
            UserEntity ue = new UserEntity()
            {
                Id = userInDb.Id, 
                ConfirmICR = model.ConfirmICR, 
                Email = model.Email,
                IsAdmin = model.IsAdmin,
                KeepLogged = false,
                UserName = model.Name
            };

            var updateStatus = await _userRepository.UpdateUserAsync(ue);
            return new JsonResult(new { statusCode = 200 });
        }
    }
}
