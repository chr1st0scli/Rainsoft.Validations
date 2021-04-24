namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given string's length is greater than a specific size.
    /// </summary>
    public class LongerValidator : ValidatorDecorator<string>
    {
        /// <summary>
        /// The length a value's length must surpass.
        /// </summary>
        protected uint length;

        /// <summary>
        /// Constructs a validator for checking that a value is longer than a certain length.
        /// </summary>
        /// <param name="length">The length a value's length must surpass.</param>
        /// <param name="validator">A string validator that can be combined with this one.</param>
        public LongerValidator(uint length, IValueValidator<string> validator = null)
            : base(validator)
        {
            this.length = length;
        }

        /// <summary>
        /// Validates that a string's length is greater than a given number. 
        /// <para>A possible nested validator is executed first if one was supplied in the constructor.</para>
        /// If value is null, then false is returned except if the nested validator does not accept null and throws an exception.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if valid, false otherwise or if <paramref name="value"/> is null.</returns>
        public override bool IsValid(string value)
        {
            if (!base.IsValid(value))
                return false;

            // A null value is certainly not longer than any uint, even 0 because null's length is considered to be 0.
            return value != null && value.Length > length;
        }

        /// <summary>
        /// Returns a string representation of this validator.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(LongerValidator)} {length}";
    }
}
