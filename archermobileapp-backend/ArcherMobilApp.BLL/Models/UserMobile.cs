using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ArcherMobilApp.BLL.Models
{
    public class UserMobile
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string NewEmail { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
