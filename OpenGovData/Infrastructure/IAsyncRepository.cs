using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IAsyncRepository<T, in K> where T : class
    {
        /// <summary>
        /// Adds the specified entity to underlying collection.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Returns an IQueryable of all items of type T.
        /// </summary>
        /// <param name="includeProperties">
        /// An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.
        /// </param>
        /// <returns>Returns an IQueryable of all items of type T; none if no objects found</returns>
        Task<IQueryable<T>> FindAllAsync(params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Returns an IEnumerable of items of type T.
        /// </summary>
        /// <param name="predicate">
        /// A predicate to limit the items being returned.
        /// </param>
        /// <param name="includeProperties">
        /// An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.
        /// </param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="id">The unique identifier</param>
        /// <param name="includeProperties">
        /// An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.
        /// </param>
        /// <returns>The requested item when found, or null otherwise.</returns>
        Task<T> FindByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task RemoveAsync(T entity);

        /// <summary>
        /// Removes the specified entity identified by id parameter.
        /// </summary>
        /// <param name="iD">The i d.</param>
        /// <returns></returns>
        Task RemoveAsync(K iD);
    }
}