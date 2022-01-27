using System;
using System.Collections.Generic;
using System.Text;

namespace Archer.AMA.Core.Pramaneters
{
    public class FilterItem
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public FilterItemMatchMode Mode { get; set; }

    }
}
