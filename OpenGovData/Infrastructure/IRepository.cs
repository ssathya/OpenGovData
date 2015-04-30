using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure
{
    public interface IRepository<T, in K> where T : class
    {
        #region Public Methods

        /// <summary>
        /// Adds the specified entity to underlying collection.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(T entity);

        /// <summary>
        /// Returns an IQueryable of all items of type T.
        /// </summary>
        /// <param name="includeProperties">
        /// An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.
        /// </param>
        /// <returns></returns>
        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

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
        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="id">The unique identifier</param>
        /// <param name="includeProperties">
        /// An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.
        /// </param>
        /// <returns>The requested item when found, or null otherwise.</returns>
        T FindById(K id, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Remove(T entity);

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Remove(K id);

        #endregion Public Methods
    }
}