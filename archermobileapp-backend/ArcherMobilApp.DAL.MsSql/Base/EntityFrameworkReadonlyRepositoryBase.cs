using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Archer.AMA.Entity.Base;
using Archer.AMA.DAL.Contract.Base;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System;
using Archer.AMA.Core.Pramaneters;

namespace Archer.AMA.DAL.EntityFramework.Base
{

    /// <summary>
    /// Provides base methods implementation based on EntityFramework Core and Dapper. Depends on readonly data accesslayer
    /// </summary>
    /// <typeparam name="TEntity">The entity type. Should be inherited from <see cref="EntityBase{TID}"/></typeparam>
    /// <typeparam name="TID">The the type of entity Id </typeparam>
    /// <typeparam name="TRepository">Data access layer</typeparam>
    public abstract class EntityFrameworkReadonlyRepositoryBase<TEntity, TID> : IReadonlyRepository<TEntity, TID>
            where TEntity : EntityBase<TID>
    {

        protected readonly DbContextOptions Options;
        protected EntityFrameworkReadonlyRepositoryBase(DbContextOptions<ArcherContext> options)
        {
            Options = options;
        }

        private Expression CreateExpression(FilterItemMatchMode filterItemMode, object filterItemValue, MemberExpression propertyAccess) =>
            filterItemMode switch
            {
                FilterItemMatchMode.Equals => Expression.Equal(propertyAccess, Expression.Constant(filterItemValue)),
                FilterItemMatchMode.Contains => Expression.Call(propertyAccess, "Contains", null, Expression.Constant(filterItemValue)),
                _ => throw new NotImplementedException()
            };

        /// <summary>
        /// Creates db context for accessing to storage.
        /// </summary>
        /// <param name="queryTrackingBehavior"></param>
        /// <returns></returns>
        internal ArcherContext<TEntity, TID> CreateContext(QueryTrackingBehavior queryTrackingBehavior = QueryTrackingBehavior.NoTracking)
        {
            return new ArcherContext<TEntity, TID>(Options, queryTrackingBehavior);
        }

        /// <summary>
        /// Creates db context for accessing to storage.
        /// </summary>
        /// <typeparam name="TLocalEntity"></typeparam>
        /// <typeparam name="TLocalID"></typeparam>
        /// <param name="queryTrackingBehavior"></param>
        /// <returns></returns>
        internal ArcherContext<TLocalEntity, TLocalID> CreateContext<TLocalEntity, TLocalID>(QueryTrackingBehavior queryTrackingBehavior = QueryTrackingBehavior.TrackAll) where TLocalEntity : class
        {
            return new ArcherContext<TLocalEntity, TLocalID>(Options, queryTrackingBehavior);
        }

        /// <summary>
        /// Asynchronously returns all entities from storage set.
        /// </summary>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <param name="filterValues">Additional filter array</param>
        /// <returns>All entities from storage set</returns>
        public virtual async Task<IEnumerable<TEntity>> AllAsync(string executorIdentity, params object[] filterValues)
        {
            using (var context = CreateContext())
            {
                return await context.Entities.ToListAsync();
            }
        }

        /// <summary>
        /// Asynchronously returns paginated entities from storage set.
        /// </summary>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <param name="filterParams">Pagination params</param>
        /// <param name="filterValues">Additional filter array</param>
        /// <returns>Paginated entities from storage set</returns>
        public virtual async Task<Tuple<long, IEnumerable<TEntity>>> AllPaginatedAsync(string executorIdentity, FilterParams filterParams, params object[] filterValues)
        {
            using (var context = CreateContext())
            {               
                IQueryable<TEntity> result = context.Entities;

                if (filterParams.Filter != null)
                    result = FilterBy(result, filterParams.Filter);

                var elementsCount = await result.CountAsync();

                if (filterParams.Sort != null)
                    result = OrderBy(result, filterParams.Sort);

                if (filterParams.StartRow > 0)
                    result = result.Skip(filterParams.StartRow);
                if ((filterParams.EndRow - filterParams.StartRow) > 0)
                    result = result.Take(filterParams.EndRow - filterParams.StartRow);

                return Tuple.Create<long, IEnumerable<TEntity>>(elementsCount, await result.ToListAsync());
            }
        }


        /// <summary>
        /// Asynchronously returns the specified entity by id from storage set.
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="executorIdentity">Current executor identity</param>
        /// <param name="filterValues">Additional filter array</param>
        /// <returns>Specified entity by id from storage set.</returns>
        public virtual async Task<TEntity> GetByIdAsync(TID id, string executorIdentity, params object[] filterValues)
        {
            using (var context = CreateContext())
            {
                return await context.Entities.SingleOrDefaultAsync(obj => obj.Id.Equals(id));
            }
        }


        /// <summary>
        /// Filters the specified Linq expression.
        /// </summary>
        /// <param name="source">Source for filtering</param>
        /// <param name="filterItems">Filter items</param>
        /// <returns>Ordered Collection</returns>
        protected virtual IQueryable<TEntity> FilterBy(IQueryable<TEntity> source, IEnumerable<FilterItem> filterItems)
        {
            var type = typeof(TEntity);
            var parameter = Expression.Parameter(typeof(TEntity), "entity");

            Expression filterExpression = null;

            foreach (var filterItem in filterItems)
            {
                var property = type.GetProperty(filterItem.Field, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.GetProperty);
                if (property == null)
                    continue;

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var equality = CreateExpression(filterItem.Mode, filterItem.Value, propertyAccess);

                if (filterExpression == null)
                    filterExpression = equality;
                else
                    filterExpression = Expression.And(filterExpression, equality);
            }

            if (filterExpression != null)
            {
                var whereBody = Expression.Lambda<Func<TEntity, bool>>(filterExpression, parameter);

                source = source.Where(whereBody);
            }

            return source;

        }


        /// <summary>
        /// Orders the specified Linq expression.
        /// </summary>
        /// <param name="source">Source for ordering</param>
        /// <param name="orderItems">Order items</param>
        /// <returns>Ordered Collection</returns>
        protected virtual IQueryable<TEntity> OrderBy(IQueryable<TEntity> source, IEnumerable<SortOrderItem> orderItems)
        {
            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type, "p");

            foreach (var orderItem in orderItems)
            {
                string command = orderItem.Desc ? "OrderByDescending" : "OrderBy";


                var property = type.GetProperty(orderItem.Prop, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.GetProperty);
                if (property == null)
                    continue;

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                var resultExpression = Expression.Call(typeof(Queryable), command,
                                                       new[] { type, property.PropertyType },
                                                       source.AsQueryable().Expression,
                                                       Expression.Quote(orderByExpression));


                source = source.AsQueryable().Provider.CreateQuery<TEntity>(resultExpression);
            }

            return source;
        }

    }
}
