using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Infrastructure
{
    public abstract class CollectionBase<T> : Collection<T>, IList<T>
    {
        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionBase{T}"/> class.
        /// </summary>
        protected CollectionBase()
            : base(new List<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionBase{T}"/> class.
        /// </summary>
        /// <param name="initialList">The initial list.</param>
        protected CollectionBase(IList<T> initialList)
            : base(initialList)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionBase{T}"/> class.
        /// </summary>
        /// <param name="initialBase">The initial base.</param>
        protected CollectionBase(CollectionBase<T> initialBase)
            : base(initialBase)
        {
        }

        #endregion Protected Constructors

        #region Public Methods

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">collection;Parameter collection is null</exception>
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection", "Parameter collection is null");
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Sorts the specified comparer.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public void Sort(IComparer<T> comparer)
        {
            var list = Items as List<T>;
            if (list != null)
                list.Sort(comparer);
        }

        /// <summary>
        /// Sorts this instance.
        /// </summary>
        public void Sort()
        {
            var list = Items as List<T>;
            if (list != null)
                list.Sort();
        }

        #endregion Public Methods
    }
}