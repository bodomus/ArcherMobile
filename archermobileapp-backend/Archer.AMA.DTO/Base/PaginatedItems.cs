using System;
using System.Collections.Generic;
using System.Text;
using Archer.AMA.DTO.Base;

namespace Archer.AMA.DTO.Base
{
    /// <summary>
    /// Provides container for paginated collection
    /// </summary>
    /// <typeparam name="TDTO"></typeparam>
    /// <typeparam name="TID"></typeparam>
    public class PaginatedItems<TDTO, TID> where TDTO : DataTransferObjectBase<TID>
    {
        public PaginatedItems(long totalRecords, IEnumerable<TDTO> items)
        {
            Items = items;
            TotalRecords = totalRecords;
        }

        /// <summary>
        /// Total records.
        /// </summary>
        public long TotalRecords { get; }

        /// <summary>
        /// Collection of <see cref="TDTO"/>.
        /// </summary>
        public IEnumerable<TDTO> Items { get; }

    }
}
