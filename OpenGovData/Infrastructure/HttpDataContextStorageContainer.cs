using System.Web;

namespace Infrastructure
{
    public class HttpDataContextStorageContainer<T> : IDataContextStorageContainer<T> where T : class
    {
        #region Private Fields

        private const string DataContextKey = "DataContext";

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Clears the object from the container.
        /// </summary>
        public void Clear()
        {
            if (HttpContext.Current.Items.Contains(DataContextKey))
                HttpContext.Current.Items.Remove(DataContextKey);
        }

        /// <summary>
        /// Returns an object from the container when it exists. Returns null otherwise.
        /// </summary>
        /// <returns></returns>
        public T GetDataContext()
        {
            T objectContext = null;
            if (HttpContext.Current.Items.Contains(DataContextKey))
                objectContext = (T)HttpContext.Current.Items[DataContextKey];
            return objectContext;
        }

        /// <summary>
        /// Stores the object in HttpContext.Current.Items.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        public void Store(T objectContext)
        {
            if (HttpContext.Current.Items.Contains(DataContextKey))
                HttpContext.Current.Items[DataContextKey] = objectContext;
            else
                HttpContext.Current.Items.Add(DataContextKey, objectContext);
        }

        #endregion Public Methods
    }
}