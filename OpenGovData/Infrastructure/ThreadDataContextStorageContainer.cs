using System;
using System.Collections;
using System.Threading;

namespace Infrastructure
{
    public class ThreadDataContextStorageContainer<T> : IDataContextStorageContainer<T> where T : class
    {
        #region Private Fields

        private static readonly Hashtable StoredContext = new Hashtable();

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            if (StoredContext.Contains(GetThreadName()))
                StoredContext[GetThreadName()] = null;
        }

        /// <summary>
        /// Gets the data context.
        /// </summary>
        /// <returns></returns>
        public T GetDataContext()
        {
            if (StoredContext != null && StoredContext.Contains(GetThreadName()))
                return (T)StoredContext[GetThreadName()];
            return null;
        }

        /// <summary>
        /// Stores the specified object context.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        public void Store(T objectContext)
        {
            if (StoredContext.Contains(GetThreadName()))
                StoredContext[GetThreadName()] = objectContext;
            else
                StoredContext.Add(GetThreadName(), objectContext);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Gets the name of the thread.
        /// </summary>
        /// <returns></returns>
        private static string GetThreadName()
        {
            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = Guid.NewGuid().ToString();
            return Thread.CurrentThread.Name;
        }

        #endregion Private Methods
    }
}