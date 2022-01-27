using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Archer.AMA.Entity.Base;

namespace Archer.AMA.Entity
{
    [Table("Appoinments")]
    public class AppoinmentEntity : EntityBase<int?>
    {
        [Required]
        public string Appoinment { get; set; }

        [Required]
        public string OwnerId { get; set; }
        
        [Required]
        public int RoomId { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateInserted { get; set; }
        [JsonIgnore]
        public DateTime? DateUpdated { get; set; }
        [JsonIgnore]
        public DateTime? DateDeleted { get; set; }
    }
}
