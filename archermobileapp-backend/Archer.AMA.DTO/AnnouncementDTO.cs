using Archer.AMA.DTO.Base;
using System;

namespace Archer.AMA.DTO
{
    public class AnnouncementDTO : DataTransferObjectBase<int?>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public int AnnouncmentTypeId { get; set; }
        public string UserCreatetorId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? PublishDate { get; set; }
    }
}
