using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ArcherMobilApp.BLL.Models
{
    public class Document
    {
        [Key]
        [Required]
        [NotNull]
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Uri { get; set; }
        public int DocumentTypeId { get; set; }

    }
}
