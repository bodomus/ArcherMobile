using Archer.AMA.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArcherMobilApp.DAL.MsSql.Models
{
    [Table("AspNetUsers")]
    public class AspNetUsersEntity : EntityBase<string>
    {
        public string  UserName { get; set; }
        public string NormalizedUserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [EmailAddress]
        public string NormalizedEmail { get; set; }
        public Boolean EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public Boolean PhoneNumberConfirmed { get; set; }
        public Boolean TwoFactorEnabled { get; set; }
        public DateTime LockoutEnd { get; set; }
        public Boolean LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

    }
}
