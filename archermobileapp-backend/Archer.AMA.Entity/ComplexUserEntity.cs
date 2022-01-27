using System;
using System.ComponentModel.DataAnnotations;

namespace ArcherMobilApp.DAL.MsSql.Models
{
    public class ComplexUserEntity
    {
        [Required]
        [Key]
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Boolean LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public Boolean IsAdmin { get; set; }
        public DateTime? LastLogin { get; set; }
        public Boolean ConfirmICR { get; set; }
    }
}
