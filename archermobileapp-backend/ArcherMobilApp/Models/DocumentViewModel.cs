using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ArcherMobilApp.Models
{
    public class DocumentViewModel
    {

        public int Id { get; set; }

        [NotNull]
        public string Title { get; set; }
        public string Description { get; set; }
        [NotNull]
        public string Uri { get; set; }
        [NotNull]
        [JsonIgnore]
        public int DocumentTypeId { get; set; }
    }
}
