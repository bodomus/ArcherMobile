using Archer.AMA.DTO.Base;
using System;

namespace Archer.AMA.DTO
{
    public class AppoinmentDTO : DataTransferObjectBase<int?>
    {
        public string Appoinment { get; set; }
        public string OwnerId { get; set; }
        public int? RoomId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}
