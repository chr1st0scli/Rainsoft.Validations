using System;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates using a method that returns a boolean value.
    /// <para>This validator is not meant to be combined with others.</para>
    /// </summary>
    public class FuncValidator : IValueValidator<Func<bool>>
    {
        /// <summary>
        /// Runs a validation using a deferred method that performs the actual validation.
        /// The method must return true for a valid case and false otherwise.
        /// </summary>
        /// <param name="value">The method to be called.</param>
        /// <returns>True if valid, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        public bool IsValid(Func<bool> value)
        {
            return value?.Invoke() ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Returns a string representation of this validator.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(FuncValidator)}";
    }
}
