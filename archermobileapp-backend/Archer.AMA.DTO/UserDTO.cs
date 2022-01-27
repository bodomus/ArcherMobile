using Archer.AMA.DTO.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Archer.AMA.DTO
{
    public class UserDTO : DataTransferObjectBase<string>
    {
        public bool ConfirmICR { get; set; }
        public DateTime? DateInserted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool KeepLogged { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime? ICRConfirmationDate { get; set; }
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
    }
}
