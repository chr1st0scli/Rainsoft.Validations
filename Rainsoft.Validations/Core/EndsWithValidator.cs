using System;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given string ends with a specific string.
    /// </summary>
    public class EndsWithValidator : ValidatorDecorator<string>
    {
        /// <summary>
        /// The desired way a value must end with.
        /// </summary>
        protected string end;

        /// <summary>
        /// Specifies if <see cref="end"/> is checked in a case sensitive manner.
        /// </summary>
        protected bool caseSensitive;

        /// <summary>
        /// Constructs a validator for checking a value's ending.
        /// </summary>
        /// <param name="end">The desired way a value must end with.</param>
        /// <param name="caseSensitive">Specifies if <paramref name="end"/> is checked in a case sensitive manner.</param>
        /// <param name="validator">A string validator that can be combined with this one.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="end"/> is null.</exception>
        public EndsWithValidator(string end, bool caseSensitive = true, IValueValidator<string> validator = null)
            : base(validator)
        {
            this.end = end ?? throw new ArgumentNullException(nameof(end));
            this.caseSensitive = caseSensitive;
        }

        /// <summary>
        /// Validates that a string ends with a specific way.
        /// <para>A possible nested validator is executed first if one was supplied in the constructor.</para>
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if valid, false otherwise or if <paramref name="value"/> is null.</returns>
        public override bool IsValid(string value)
        {
            if (!base.IsValid(value) || value == null)
                return false;

            return value.EndsWith(end, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Returns a string representation of this validator.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(EndsWithValidator)} {end}";
    }
}
