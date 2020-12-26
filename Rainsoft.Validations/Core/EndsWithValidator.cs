using System;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given string ends with a specific string.
    /// </summary>
    public class EndsWithValidator : ValidatorDecorator<string>
    {
        protected string end;

        /// <summary>
        /// Constructs a validator for checking a value's ending.
        /// </summary>
        /// <param name="end">The desired way a value must end with.</param>
        /// <param name="validator">A string validator that can be combined with this one.</param>
        /// <exception cref="ArgumentNullException">Thrown if end is null.</exception>
        public EndsWithValidator(string end, IValidator<string> validator = null)
            : base(validator)
        {
            this.end = end ?? throw new ArgumentNullException(nameof(end));
        }

        /// <summary>
        /// Validates that a string ends with a specific way.
        /// A possible nested validator is executed first if one was supplied in the constructor.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        public override bool IsValid(string value)
        {
            if (!base.IsValid(value))
                return false;

            return value?.EndsWith(end) ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
