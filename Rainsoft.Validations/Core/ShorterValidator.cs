namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given string's length is less than a specific size.
    /// </summary>
    public class ShorterValidator : ValidatorDecorator<string>
    {
        /// <summary>
        /// The length that must surpass a value's length.
        /// </summary>
        protected uint length;

        /// <summary>
        /// Constructs a validator for checking that a value is shorter than a certain length.
        /// </summary>
        /// <param name="length">The length that must surpass a value's length.</param>
        /// <param name="validator">A string validator that can be combined with this one.</param>
        public ShorterValidator(uint length, IValueValidator<string> validator = null)
            : base(validator)
        {
            this.length = length;
        }

        /// <summary>
        /// Validates that a string's length is smaller than a given number. 
        /// <para>A possible nested validator is executed first if one was supplied in the constructor.</para>
        /// If value is null, then true is returned except if the nested validator does not accept null and throws an exception.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>
        /// True if valid or <paramref name="value"/> is null and the intended length is greater than 0.
        /// False if invalid or <paramref name="value"/> is null and the intended length is 0.
        /// </returns>
        public override bool IsValid(string value)
        {
            if (!base.IsValid(value))
                return false;

            // A null value is shorter than any other uint but zero, because null's length is considered to be 0.
            if (value == null)
                return length > 0;

            return value.Length < length;
        }

        /// <summary>
        /// Returns a string representation of this validator.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(ShorterValidator)} {length}";
    }
}
