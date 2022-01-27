using System.Collections.Generic;

namespace ArcherMobilApp.Models.AuxiliaryModels
{
    public class AgGridModel
    {
        public int StartRow { get; set; }
        public int EndRow { get; set; }

        public IEnumerable<AgSortModel> Sort { get; set; }
    }
}

