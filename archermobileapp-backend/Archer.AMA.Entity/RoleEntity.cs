using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ArcherMobilApp.DAL.MsSql.Models
{
    [Table("AspNetRoles")]
    public class RoleEntity
    {
        [Key]
        public string Id { get; set; }
        
        public  string  Name { get; set; }
        [JsonIgnore]
        public  string NormalizedName { get; set; }
        [JsonIgnore]
        public  string ConcurrencyStamp { get; set; }
    }
}
