using System.Web;

namespace Infrastructure
{
    public static class DataContextStorageFactory<T> where T : class
    {
        #region Private Fields

        private static IDataContextStorageContainer<T> _dataContextStorageContainer;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Creates a new container that uses HttpContext.Current.Items (when HttpContext.Current is not null) or Thread.
        /// </summary>
        /// <returns>A contact storage container to store objects. </returns>
        public static IDataContextStorageContainer<T> CreateStorageContainer()
        {
            if (_dataContextStorageContainer != null) return _dataContextStorageContainer;
            if (HttpContext.Current == null)
                return _dataContextStorageContainer =
                    new ThreadDataContextStorageContainer<T>();
            else
                return _dataContextStorageContainer =
                    new HttpDataContextStorageContainer<T>();
        }

        #endregion Public Methods
    }
}