using System;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given value is greater than, or optionally equal to, a margin.
    /// </summary>
    /// <typeparam name="T">The type of the margin and value.</typeparam>
    public class GreaterValidator<T> : ValidatorDecorator<IComparable<T>>
    {
        /// <summary>
        /// The margin that a value must surpass.
        /// </summary>
        protected T margin;

        /// <summary>
        /// True for also checking for an equality to <see cref="margin"/>.
        /// </summary>
        protected bool orEqualTo;

        /// <summary>
        /// Constructs a validator for checking that a value is greater than, or optionally equal to, a <paramref name="margin"/>.
        /// </summary>
        /// <param name="margin">The margin that a value must surpass or optionally be equal to.</param>
        /// <param name="orEqualTo">If true, the validator checks that a value is greater than or equal to <paramref name="margin"/>. If false, it checks that a value is only greater than <paramref name="margin"/>.</param>
        /// <param name="validator">A validator of the same type that can be combined with this one.</param>
        /// <exception cref="ArgumentNullException">Thrown if margin is null.</exception>
        public GreaterValidator(T margin, bool orEqualTo = false, IValueValidator<IComparable<T>> validator = null)
            : base(validator)
        {
            if (margin == null)
                throw new ArgumentNullException(nameof(margin));
            this.margin = margin;
            this.orEqualTo = orEqualTo;
        }

        /// <summary>
        /// Validates that <paramref name="value"/> is greater than (or equal to, if <see cref="orEqualTo"/> is true) the specified <see cref="margin"/>.
        /// <para>A possible nested validator is executed first if one was supplied in the constructor.</para>
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

            int res = value.CompareTo(margin);

            return orEqualTo ? res == 0 || res == 1 : res == 1;
        }

        /// <summary>
        /// Returns a string representation of this validator.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(GreaterValidator<T>)} {margin}";
    }
}
