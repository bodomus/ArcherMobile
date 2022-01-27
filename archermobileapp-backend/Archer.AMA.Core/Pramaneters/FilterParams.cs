using System.Collections.Generic;

namespace Archer.AMA.Core.Pramaneters
{
    public class FilterParams
    {
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public IEnumerable<SortOrderItem> Sort {get;set;}
        public IEnumerable<FilterItem> Filter { get; set; }
    }
}
