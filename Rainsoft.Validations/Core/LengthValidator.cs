using System;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given string is of a certain length.
    /// </summary>
    public class LengthValidator : ValidatorDecorator<string>
    {
        /// <summary>
        /// The desired length of a value.
        /// </summary>
        protected uint length;

        /// <summary>
        /// Constructs a validator for checking a value's length.
        /// </summary>
        /// <param name="length">The desired value's length.</param>
        /// <param name="validator">A string validator that can be combined with this one.</param>
        public LengthValidator(uint length, IValueValidator<string> validator = null)
            : base(validator)
        {
            this.length = length;
        }

        /// <summary>
        /// Validates that a string is of a certain length.
        /// <para>A possible nested validator is executed first if one was supplied in the constructor.</para>
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>
        /// True if valid or <paramref name="value"/> is null and the intended length is 0.
        /// False if invalid or <paramref name="value"/> is null and the intended length is greater than 0.
        /// </returns>
        public override bool IsValid(string value)
        {
            if (!base.IsValid(value))
                return false;

            // Safe to say since length is uint.
            if (value == null)
                return length == 0;

            return value.Length == length;
        }

        /// <summary>
        /// Returns a string representation of this validator.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(LengthValidator)} {length}";
    }
}
