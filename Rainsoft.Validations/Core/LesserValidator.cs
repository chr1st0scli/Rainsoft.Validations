using System;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given value is less than, or optionally equal to, a margin.
    /// </summary>
    /// <typeparam name="T">The type of the margin and value.</typeparam>
    public class LesserValidator<T> : ValidatorDecorator<IComparable<T>>
    {
        /// <summary>
        /// The margin that a value must be less than.
        /// </summary>
        protected T margin;

        /// <summary>
        /// True for also checking for an equality to <see cref="margin"/>.
        /// </summary>
        protected bool orEqualTo;

        /// <summary>
        /// Constructs a validator for checking that a value is less than, or optionally equal to, a <paramref name="margin"/>.
        /// </summary>
        /// <param name="margin">The margin that a value must be less than.</param>
        /// <param name="orEqualTo">If true, the validator checks that a value is less than or equal to <paramref name="margin"/>. If false, it checks that a value is only less than <paramref name="margin"/>.</param>
        /// <param name="validator">A validator of the same type that can be combined with this one.</param>
        /// <exception cref="ArgumentNullException">Thrown if margin is null.</exception>
        public LesserValidator(T margin, bool orEqualTo = false, IValidator<IComparable<T>> validator = null)
            : base(validator)
        {
            if (margin == null)
                throw new ArgumentNullException(nameof(margin));
            this.margin = margin;
            this.orEqualTo = orEqualTo;
        }

        /// <summary>
        /// Validates that <paramref name="value"/> is less than (or equal to, if <see cref="orEqualTo"/> is true) the specified <see cref="margin"/>.
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

            return orEqualTo ? res == 0 || res == -1 : res == -1;
        }
    }
}
