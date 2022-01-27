using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Archer.AMA.BLL.Contract.Base;
using Archer.AMA.Core.Pramaneters;
using Archer.AMA.DAL.Contract.Base;
using Archer.AMA.DTO.Base;
using Archer.AMA.Entity.Base;

namespace Archer.AMA.BLL.Base
{

    /// <summary>
    /// Provides base methods implementation set for readonly data access layer
    /// </summary>
    /// <typeparam name="TEntity">The entity type. Should be inherited from <see cref="EntityBase{TID}"/></typeparam>
    /// <typeparam name="TID">The the type of entity Id </typeparam>
    /// <typeparam name="TRepository">Data access layer</typeparam>
    public abstract class ReadonlyRepositoryServiceBase<TRepository, TEntity, TDTO, TID> : IReadonlyRepositoryService<TDTO, TID>
        where TRepository : IReadonlyRepository<TEntity, TID> 
        where TEntity : EntityBase<TID>
        where TDTO : DataTransferObjectBase<TID>
    {

        protected ReadonlyRepositoryServiceBase(TRepository repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }
        protected TRepository Repository { get; set; }
        protected IMapper Mapper { get; set; }


        /// <summary>
        /// Map To DTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected TDTO ToDTO(TEntity entity)
        {
            return Mapper.Map<TDTO>(entity);
        }

        /// <summary>
        /// Asynchronously returns all entities from storage set.
        /// </summary>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <param name="filterValues">Additional filter array</param>
        /// <returns>All entities from storage set</returns>
        public virtual async Task<IEnumerable<TDTO>> AllAsync(string executorIdentity, params object[] filterValues)
        {
            var result = await Repository.AllAsync(executorIdentity, filterValues);
            return result.Select(obj => ToDTO(obj));
        }

        /// <summary>
        /// Asynchronously returns paginated entities from storage set.
        /// </summary>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <param name="paginationParams">Pagination params</param>
        /// <param name="filterValues">Additional filter array</param>
        /// <returns>Paginated entities from storage set</returns>
        public virtual async Task<PaginatedItems<TDTO, TID>> AllPaginatedAsync(string executorIdentity, FilterParams filterParams, params object[] filterValues)
        {
            var result = await Repository.AllPaginatedAsync(executorIdentity, filterParams, filterValues);
            return new PaginatedItems<TDTO, TID>(result.Item1, result.Item2.Select(obj => ToDTO(obj)));
        }

        /// <summary>
        /// Asynchronously returns the specified entity by id from storage set.
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <param name="filterValues">Additional filter array</param>
        /// <returns>Specified entity by id from storage set.</returns>
        public virtual async Task<TDTO> GetByIdAsync(TID id, string executorIdentity, params object[] filterValues)
        {
            var result = await Repository.GetByIdAsync(id, executorIdentity, filterValues);
            return ToDTO(result);
        }
    }
}
