using System;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given <see cref="DateTime"/> is in the past or present. 
    /// Present is DateTime.Now at the time of validation.
    /// </summary>
    public class NotFutureValidator : ValidatorDecorator<DateTime>
    {
        /// <summary>
        /// Constructs a validator for checking if a <see cref="DateTime"/> does not belong to the future.
        /// </summary>
        /// <param name="validator">A DateTime validator that can be combined with this one.</param>
        public NotFutureValidator(IValueValidator<DateTime> validator = null)
            : base(validator)
        {
        }

        /// <summary>
        /// Validates that <paramref name="value"/> does not belong to the future.
        /// <para>A possible nested validator is executed first if one was supplied in the constructor.</para>
        /// </summary>
        /// <param name="value">The DateTime to validate. If time should be ignored, only the date part can be included.</param>
        /// <returns>True if value is less than or equal to DateTime.Now.</returns>
        public override bool IsValid(DateTime value)
        {
            if (!base.IsValid(value))
                return false;

            return value <= DateTime.Now;
        }

        /// <summary>
        /// Returns a string representation of this validator.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(NotFutureValidator)}";
    }
}
