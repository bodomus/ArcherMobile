using Archer.AMA.DTO.Base;
using System.ComponentModel.DataAnnotations;

namespace Archer.AMA.DTO
{
    public class JobOpportunityDTO : DataTransferObjectBase<int?>
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Responsibilities { get; set; }
        [Required]
        public string Requirements { get; set; }
        [Required]
        public string StandOut { get; set; }
        [Required]
        public string RecruiterContacts { get; set; }
        [Required]
        public bool IsArchive { get; set; }
    }
}
