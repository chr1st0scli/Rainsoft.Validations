using System;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given <see cref="DateTime"/> is in the future. Present is DateTime.Now at the time of validation.
    /// </summary>
    public class FutureValidator : ValidatorDecorator<DateTime>
    {
        /// <summary>
        /// Constructs a validator for checking if a DateTime belongs to the future.
        /// </summary>
        /// <param name="validator">A DateTime validator that can be combined with this one.</param>
        public FutureValidator(IValidator<DateTime> validator = null)
            : base(validator)
        {
        }

        /// <summary>
        /// Validates that <paramref name="value"/> belongs to the future.
        /// <para>A possible nested validator is executed first if one was supplied in the constructor.</para>
        /// </summary>
        /// <param name="value">The DateTime to validate.</param>
        /// <returns>True if value is greater than DateTime.Now.</returns>
        public override bool IsValid(DateTime value)
        {
            if (!base.IsValid(value))
                return false;

            return value > DateTime.Now;
        }
    }
}
