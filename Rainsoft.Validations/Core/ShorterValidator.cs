namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given string's length is less than a specific size.
    /// </summary>
    public class ShorterValidator : ValidatorDecorator<string>
    {
        protected uint length;

        /// <summary>
        /// Constructs a validator for checking that a value is shorter than a certain length.
        /// </summary>
        /// <param name="length">The length that must surpass a value's length.</param>
        /// <param name="validator">A string validator that can be combined with this one.</param>
        public ShorterValidator(uint length, IValidator<string> validator = null)
            : base(validator)
        {
            this.length = length;
        }

        /// <summary>
        /// Validates that a string's length is smaller than a given number. A possible nested validator 
        /// is executed first if one was supplied in the constructor. If value is null, then true is
        /// returned except if the nested validator does not accept null and throws an exception.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if value is null or valid, false otherwise.</returns>
        public override bool IsValid(string value)
        {
            if (!base.IsValid(value))
                return false;

            // A null value is certainly shorter than anything.
            return value == null || value.Length < length;
        }
    }
}
