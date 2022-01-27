using Archer.AMA.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ArcherMobilApp.DAL.MsSql.Models
{
    [Table("Announcements")]
    public class AnnouncementEntity : EntityBase<int?>
    {
	    [Required]
        public string Title { get; set; }
	 
        
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        
        [Required]
        public int AnnouncmentTypeId { get; set; }

        public string  UserCreatetorId { get; set; }
        public DateTime? Date { get; set; }

        public DateTime? PublishDate { get; set; }
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateInserted { get; set; }
        [JsonIgnore]
        public DateTime? DateUpdated { get; set; }
        [JsonIgnore]
        public DateTime? DateDeleted { get; set; }
    }
}
