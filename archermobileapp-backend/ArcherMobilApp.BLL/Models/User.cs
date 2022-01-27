using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ArcherMobilApp.BLL.Models
{
    public class User
    {
        [Required]
        [NotNull]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        [DefaultValue(false)]
        public bool ConfirmICR { get; set; }
    }
}
