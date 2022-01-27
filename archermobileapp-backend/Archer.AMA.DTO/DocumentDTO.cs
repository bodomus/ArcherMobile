using System.ComponentModel.DataAnnotations;
using Archer.AMA.DTO.Base;

namespace Archer.AMA.DTO
{
    public class DocumentDTO : DataTransferObjectBase<int?>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Uri { get; set; }
        [Required]
        public int DocumentTypeId { get; set; }
    }
}
