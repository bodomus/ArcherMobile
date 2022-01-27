using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using AutoMapper;

using Archer.AMA.BLL.Contract.Base;
using Archer.AMA.DAL.Contract.Base;
using Archer.AMA.DTO.Base;
using Archer.AMA.Entity.Base;

namespace Archer.AMA.BLL.Base
{
    public class RepositoryServiceBase<TRepository, TEntity, TDTO, TID> : ReadonlyRepositoryServiceBase<TRepository, TEntity, TDTO, TID>, IRepositoryService<TDTO, TID>
        where TRepository : IEditableRepository<TEntity, TID>
        where TEntity : EntityBase<TID>
        where TDTO : DataTransferObjectBase<TID>
    {
        protected RepositoryServiceBase(TRepository repository, IMapper mapper)
            :base(repository, mapper)
        {

        }

        /// <summary>
        /// Map To Enity
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected TEntity ToEntity(TDTO dto)
        {
            return Mapper.Map<TDTO, TEntity>(dto);
        }

        /// <summary>
        /// Asynchronously removes the specified entity by id from storage set.
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <returns>True if entity found and deleted</returns>
        public async Task<bool> DeleteAsync(TID id, string executorIdentity)
        {
            return await Repository.DeleteAsync(id, executorIdentity);
        }

        /// <summary>
        /// Creates a new istance of entity.
        /// </summary>
        /// <returns></returns>
        public TDTO New()
        {
            return ToDTO(Repository.New());
        }

        /// <summary>
        /// Asynchronously saves the specified entity to the storage set
        /// </summary>
        /// <param name="entity">Specified entity</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <returns>Saved entity</returns>
        public async Task<TDTO> SaveAsync(TDTO dto, string executorIdentity)
        {
            return ToDTO(await Repository.SaveAsync(ToEntity(dto), executorIdentity));
        }

        /// <summary>
        /// Asynchronously saves the specified entity range to the storage set
        /// </summary>
        /// <param name="range">Specified entity range</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <returns></returns>
        public async Task<IEnumerable<TDTO>> SaveRangeAsync(IEnumerable<TDTO> range, string executorIdentity)
        {
            return (await Repository.SaveRangeAsync(range.Select(obj => ToEntity(obj)), executorIdentity)).Select(obj => ToDTO(obj));
        }
    }
}
