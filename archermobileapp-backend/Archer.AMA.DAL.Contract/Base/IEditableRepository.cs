using Archer.AMA.Entity.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archer.AMA.DAL.Contract.Base
{
    public interface IEditableRepository<TEntity, TID> : IReadonlyRepository<TEntity, TID>
        where TEntity : EntityBase<TID>
    {
        /// <summary>
        /// Creates a new istance of entity.
        /// </summary>
        /// <returns></returns>
        TEntity New();

        /// <summary>
        /// Asynchronously saves the specified entity to the storage set
        /// </summary>
        /// <param name="entity">Specified entity</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <returns>Saved entity</returns>
        Task<TEntity> SaveAsync(TEntity entity, string executorIdentity);

        /// <summary>
        /// Asynchronously removes the specified entity by id from storage set.
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <returns>True if entity found and deleted</returns>
        Task<bool> DeleteAsync(TID id, string executorIdentity);

        /// <summary>
        /// Asynchronously saves the specified entity range to the storage set
        /// </summary>
        /// <param name="range">Specified entity range</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> SaveRangeAsync(IEnumerable<TEntity> range, string executorIdentity);

    }
}
