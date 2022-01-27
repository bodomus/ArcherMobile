using System;

namespace ArcherMobilApp.Models
{
    public class Use 
    {
        public Boolean ConfirmICR { get; set; }
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
