using System;
using System.Collections.Generic;
using System.Text;

namespace Archer.AMA.DTO.Base
{
    public class DataTransferObjectBase<TID>
    {
        public TID Id { get; set; }
    }
}
