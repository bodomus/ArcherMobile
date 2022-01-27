
using Archer.AMA.DTO.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archer.AMA.BLL.Contract.Base
{
    public interface IRepositoryService<TDTO, TID> : IReadonlyRepositoryService<TDTO, TID>
        where TDTO : DataTransferObjectBase<TID>
    {
        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <returns></returns>
        TDTO New();


        /// <summary>
        /// Asynchronously saves the specified entity to the storage set
        /// </summary>
        /// <param name="entity">Specified entity</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <returns>Saved entity</returns>
        Task<TDTO> SaveAsync(TDTO dto, string executorIdentity);

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
        /// <returns>The range of the saved items.</returns>
        Task<IEnumerable<TDTO>> SaveRangeAsync(IEnumerable<TDTO> range, string executorIdentity);

    }
}
