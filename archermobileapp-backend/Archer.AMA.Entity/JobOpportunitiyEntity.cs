using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

using Archer.AMA.Entity.Base;

namespace Archer.AMA.Entity
{
    [Table("JobOpportunities")]
    public class JobOpportunityEntity : EntityBase<int?>
    {
        [NotNull]
        public string Title { get; set; }
        public string Description { get; set; }
        [NotNull]
        public string Responsibilities { get; set; }
        [NotNull]
        public string Requirements { get; set; }
        [NotNull]
        public string StandOut { get; set; }
        [NotNull]
        public string RecruiterContacts { get; set; }
        [NotNull]
        public bool IsArchive { get; set; }

        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateInserted { get; set; }
        [JsonIgnore]
        public DateTime? DateUpdated { get; set; }
        [JsonIgnore]
        public DateTime? DateDeleted { get; set; }
    }
}
