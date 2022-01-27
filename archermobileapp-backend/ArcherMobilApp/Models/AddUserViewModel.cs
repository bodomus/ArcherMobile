using System.ComponentModel.DataAnnotations;

namespace ArcherMobilApp.Models
{
    public class AddUserViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Roles")]
        public string InputRole { get; set; }

        [Display(Name = "Confirm ICR")]
        public bool ConfirmICR { get; set; }

        [Display(Name = "Is admin")]
        public bool IsAdmin { get; set; }
    }
}
