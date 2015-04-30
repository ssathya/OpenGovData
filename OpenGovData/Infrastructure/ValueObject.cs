using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Infrastructure
{
    public abstract class ValueObject<T> : IEquatable<T>, IValidatableObject where T : ValueObject<T>
    {
        #region Public Methods

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(T other)
        {
            if ((object)other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            PropertyInfo[] pubProperties = GetType().GetProperties();
            if (pubProperties.Any())
                return pubProperties.All(p => CheckValue(p, other));
            return true;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if ((object)obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            var item = obj as ValueObject<T>;
            if ((object)item != null)
                return Equals((T)item);
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            var hashCode = 31;
            var changeMultiplier = false;
            const int index = 1;
            PropertyInfo[] pp = this.GetType().GetProperties();
            if (pp.Any())
            {
                foreach (var item in pp)
                {
                    object value = item.GetValue(this, null);
                    if ((object)value != null)
                    {
                        hashCode = hashCode * ((changeMultiplier) ? 59 : 114) + value.GetHashCode();
                        changeMultiplier = !changeMultiplier;
                    }
                    else
                    {
                        hashCode = hashCode ^ (index * 13);
                    }
                }
            }
            return hashCode;
        }

        public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<ValidationResult> Validate()
        {
            var validationErrors = new List<ValidationResult>();
            var ctx = new ValidationContext(this, null, null);
            Validator.TryValidateObject(this, ctx, validationErrors, true);
            return validationErrors;
        }

        #endregion Public Methods

        #region Private Methods

        private bool CheckValue(PropertyInfo p, T other)
        {
            var left = p.GetValue(this, null);
            var right = p.GetValue(other, null);
            if (left == null || right == null)
                return false;
            if (left is T)
                return ReferenceEquals(left, right);
            return left.Equals(right);
        }

        #endregion Private Methods

        #region Public Properties

        public int Id { get; set; }

        #endregion Public Properties
    }
}