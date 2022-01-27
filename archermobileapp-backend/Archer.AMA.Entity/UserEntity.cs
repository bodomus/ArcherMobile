using Archer.AMA.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArcherMobilApp.DAL.MsSql.Models
{
    [Table ("Users")]
    public  class UserEntity : EntityBase<string>
    {
        public Boolean ConfirmICR { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateInserted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public DateTime? LastLogin { get; set; }
        public Boolean KeepLogged { get; set; }
        public Boolean IsAdmin { get; set; }
        public DateTime? ICRConfirmationDate { get; set; }
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }

    }
}
