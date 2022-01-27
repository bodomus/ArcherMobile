using Archer.AMA.Core.Pramaneters;
using Archer.AMA.Entity.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archer.AMA.DAL.Contract.Base
{
    /// <summary>
    /// Provides base methods set for readonly data access layer
    /// </summary>
    /// <typeparam name="TEntity">The entity type. Should be inherited from <see cref="EntityBase{TID}"/></typeparam>
    /// <typeparam name="TID">The the type of entity Id </typeparam>
    public interface IReadonlyRepository<TEntity, TID> : IRepository where TEntity : EntityBase<TID>
    {
        /// <summary>
        /// Asynchronously returns all entities from storage set.
        /// </summary>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <param name="filterValues">Additional filter array</param>
        /// <returns>All entities from storage set</returns>
        Task<IEnumerable<TEntity>> AllAsync(string executorIdentity, params object[] filterValues);

        /// <summary>
        /// Asynchronously returns paginated entities from storage set.
        /// </summary>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <param name="filterParams">Pagination params</param>
        /// <param name="filterValues">Additional filter array</param>
        /// <returns>Paginated entities from storage set</returns>
        Task<Tuple<long, IEnumerable<TEntity>>> AllPaginatedAsync(string executorIdentity, FilterParams filterParams, params object[] filterValues);

        /// <summary>
        /// Asynchronously returns the specified entity by id from storage set.
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <param name="filterValues">Additional filter array</param>
        /// <returns>Specified entity by id from storage set.</returns>
        Task<TEntity> GetByIdAsync(TID id, string executorIdentity, params object[] filterValues);
    }
}
