namespace Infrastructure
{
    public interface IUnitOfWorkFactory
    {
        #region Public Methods

        /// <summary>
        /// Creates a new instance of a unit of work
        /// </summary>
        IUnitOfWork Create();

        /// <summary>
        /// Creates a new instance of a unit of work
        /// </summary>
        /// <param name="forceNew">When true, clears out any existing in-memory data storage / cache first.</param>
        IUnitOfWork Create(bool forceNew);

        #endregion Public Methods
    }
}