using Archer.AMA.Core;
using Archer.AMA.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Archer.AMA.Entity
{
    [Table("Documents")]
    public class DocumentEntity : EntityBase<int?>
    {
        [NotNull]
        public string Title { get; set; }
        public string Description { get; set; }
        [NotNull]
        public string Uri { get; set; }
        [NotNull]
        [JsonIgnore]
        public int DocumentTypeId { get; set; }
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateInserted { get; set; }
        [JsonIgnore]
        public DateTime? DateUpdated { get; set; }
        [JsonIgnore]
        public DateTime? DateDeleted { get; set; }
    }
}
