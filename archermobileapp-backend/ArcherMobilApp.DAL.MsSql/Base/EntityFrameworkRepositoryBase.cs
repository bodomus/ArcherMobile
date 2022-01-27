using Archer.AMA.DAL.Contract.Base;
using Archer.AMA.Entity.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archer.AMA.DAL.EntityFramework.Base
{
    /// <summary>
    /// Provides base methods implementation based on EntityFramework Core and Dapper. Depends on editable data accesslayer
    /// </summary>
    /// <typeparam name="TEntity">The entity type. Should be inherited from <see cref="EntityBase{TID}"/></typeparam>
    /// <typeparam name="TID">The the type of entity Id </typeparam>
    /// <typeparam name="TRepository">Data access layer</typeparam>
    public abstract class EntityFrameworkRepositoryBase<TEntity, TID> : EntityFrameworkReadonlyRepositoryBase<TEntity, TID>, IEditableRepository<TEntity, TID>
        where TEntity : EntityBase<TID>, new()
    {
        protected EntityFrameworkRepositoryBase(DbContextOptions<ArcherContext> options)
            : base(options)
        {
        }


        /// <summary>
        /// Abstract method datermines if entity is new.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract bool IsEnitytNew(TEntity entity);


        /// <summary>
        /// Asynchronously removes the specified entity by id from storage set.
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <returns>True if entity found and deleted</returns>
        public virtual async Task<bool> DeleteAsync(TID id, string executorIdentity)
        {
            using (var context = CreateContext<TEntity, TID>())
            {
                TEntity result = await GetByIdAsync(id, executorIdentity);
                if (result != null)
                {
                    context.Entities.Remove(result);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        public virtual TEntity New()
        {
            return new TEntity();
        }


        /// <summary>
        /// Asynchronously saves the specified entity to the storage set
        /// </summary>
        /// <param name="entity">Specified entity</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <returns>Saved entity</returns>
        public async Task<TEntity> SaveAsync(TEntity entity, string executorIdentity)
        {
            using (var context = CreateContext<TEntity, TID>())
            {
                return await SaveAsync(context, entity, executorIdentity);
            }
        }

        /// <summary>
        /// Asynchronously saves the specified entity to the storage set. Override this method if you need to create transaction.
        /// </summary>
        /// <param name="context">DataBaseContext</param>
        /// <param name="entity">Specified entity</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <returns>Saved entity</returns>
        internal virtual async Task<TEntity> SaveAsync(ArcherContext<TEntity, TID> context, TEntity entity, string executorIdentity)
        {
            try
            {
                if (IsEnitytNew(entity))
                {
                    context.Add(entity);
                }
                else
                {
                    context.Update(entity);
                    //context.Entities.Attach(entity);
                    //context.Entry(entity).State = EntityState.Modified;
                }

                await context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Asynchronously saves the specified entity range to the storage set
        /// </summary>
        /// <param name="range">Specified entity range</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <return
        internal virtual async Task<IEnumerable<TEntity>> SaveRangeAsync(ArcherContext<TEntity, TID> context, IEnumerable<TEntity> range, string executorIdentity)
        {
            var resultItems = new List<TEntity>();

            foreach (var item in range)
                resultItems.Add(await SaveAsync(context, item, executorIdentity));

            return resultItems;
        }


        /// <summary>
        /// Asynchronously saves the specified entity range to the storage set. Override this method if you need to create transaction.
        /// </summary>
        /// <param name="context">DataBaseContext</param>
        /// <param name="range">Specified entity range</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <return
        public virtual async Task<IEnumerable<TEntity>> SaveRangeAsync(IEnumerable<TEntity> range, string executorIdentity)
        {
            using (var context = CreateContext<TEntity, TID>())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var resultItems = await SaveRangeAsync(context, range, executorIdentity);

                    transaction.Commit();

                    return resultItems;
                }
            }
        }
    }
}
