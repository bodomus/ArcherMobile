using Archer.AMA.DTO.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Archer.AMA.DTO
{
    public class RoomDTO : DataTransferObjectBase<int?>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhysicalAddress { get; set; }
        public string LinkToGoogleCalendar { get; set; }
    }
}
