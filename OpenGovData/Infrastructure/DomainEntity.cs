using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure
{
    public abstract class DomainEntity<T> : IValidatableObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public T Id { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The first object.</param>
        /// <param name="right">The second object.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(DomainEntity<T> left, DomainEntity<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The first object.</param>
        /// <param name="right">The second object.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(DomainEntity<T> left, DomainEntity<T> right)
        {
            return Equals(left, null) ? Equals(right, null) : left.Equals(right);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DomainEntity<T>))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            var item = (DomainEntity<T>)obj;
            if (item.IsTransient() || IsTransient())
                return false;
            return item.Id.Equals(Id);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance,
        /// suitable for use in hashing algorithms and data structures like a
        /// hash table.
        /// </returns>
        public override int GetHashCode()
        {
            if (!IsTransient())
                return Id.GetHashCode() ^ 31;
            return base.GetHashCode();
        }

        /// <summary>
        /// Determines whether this instance is transient.
        /// </summary>
        /// <returns></returns>
        public bool IsTransient()
        {
            return Id.Equals(default(T));
        }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);

        public IEnumerable<ValidationResult> Validate()
        {
            var validationErrors = new List<ValidationResult>();
            var ctx = new ValidationContext(this, null, null);
            Validator.TryValidateObject(this, ctx, validationErrors, true);
            return validationErrors;
        }

        #endregion Public Methods
    }
}