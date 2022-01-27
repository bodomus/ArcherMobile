using System.Collections.Generic;
using System.Threading.Tasks;
using Archer.AMA.Core.Pramaneters;
using Archer.AMA.DTO.Base;

namespace Archer.AMA.BLL.Contract.Base
{
    /// <summary>
    /// Provides base methods set for readonly service layer
    /// </summary>
    /// <typeparam name="TDTO">The entity type. Should be inherited from <see cref="DataTransferObjectBase{TID}"/></typeparam>
    /// <typeparam name="TID">The the type of entity Id </typeparam>
    public interface IReadonlyRepositoryService<TDTO, TID> : IService
        where TDTO : DataTransferObjectBase<TID>
    {
        /// <summary>
        /// Asynchronously returns all entities from storage set.
        /// </summary>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <param name="filterValues">Additional filter array</param>
        /// <returns>All entities from storage set</returns>
        Task<IEnumerable<TDTO>> AllAsync(string executorIdentity, params object[] filterValues);

        /// <summary>
        /// Asynchronously returns paginated entities from storage set.
        /// </summary>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <param name="filterParams">Filter params</param>
        /// <param name="filterValues">Additional filter array</param>
        /// <returns>Paginated entities from storage set</returns>
        Task<PaginatedItems<TDTO, TID>> AllPaginatedAsync(string executorIdentity, FilterParams filterParams, params object[] filterValues);

        /// <summary>
        /// Asynchronously returns the specified entity by id from storage set.
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <param name="filterValues">Additional filter array</param>
        /// <returns>Specified entity by id from storage set.</returns>
        Task<TDTO> GetByIdAsync(TID id, string executorIdentity, params object[] filterValues);
    }
}
