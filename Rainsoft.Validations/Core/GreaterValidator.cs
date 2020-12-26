using System;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given value is greater than a margin.
    /// </summary>
    /// <typeparam name="T">The type of the margin and value.</typeparam>
    public class GreaterValidator<T> : ValidatorDecorator<IComparable<T>>
    {
        protected T margin;

        /// <summary>
        /// Constructs a validator for checking that a value is greater than a margin.
        /// </summary>
        /// <param name="margin">The margin that a value must surpass.</param>
        /// <param name="validator">A validator of the same type that can be combined with this one.</param>
        /// <exception cref="ArgumentNullException">Thrown if margin is null.</exception>
        public GreaterValidator(T margin, IValidator<IComparable<T>> validator = null)
            : base(validator)
        {
            if (margin == null)
                throw new ArgumentNullException(nameof(margin));
            this.margin = margin;
        }

        /// <summary>
        /// Validates that value is greater than a specified margin.
        /// A possible nested validator is executed first if one was supplied in the constructor.
        /// </summary>
        /// <param name="value">The value to validate. It can be anything that is comparable with margin.</param>
        /// <returns>True if valid, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        public override bool IsValid(IComparable<T> value)
        {
            if (!base.IsValid(value))
                return false;

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return value.CompareTo(margin) == 1;
        }
    }
}
