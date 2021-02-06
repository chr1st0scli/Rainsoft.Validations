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
        public LengthValidator(uint length, IValidator<string> validator = null)
            : base(validator)
        {
            this.length = length;
        }

        /// <summary>
        /// Validates that a string is of a certain length.
        /// A possible nested validator is executed first if one was supplied in the constructor.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        public override bool IsValid(string value)
        {
            if (!base.IsValid(value))
                return false;
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            return value.Length == length;
        }
    }
}
