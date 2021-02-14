using System;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given string starts with a specific string.
    /// </summary>
    public class StartsWithValidator : ValidatorDecorator<string>
    {
        /// <summary>
        /// The desired way a value must start with.
        /// </summary>
        protected string start;

        /// <summary>
        /// Specifies if <see cref="start"/> is checked in a case sensitive manner.
        /// </summary>
        protected bool caseSensitive;

        /// <summary>
        /// Constructs a validator for checking a value's start.
        /// </summary>
        /// <param name="start">The desired way a value must start with.</param>
        /// <param name="caseSensitive">Specifies if start is checked in a case sensitive manner.</param>
        /// <param name="validator">A string validator that can be combined with this one.</param>
        /// <exception cref="ArgumentNullException">Thrown if start is null.</exception>
        public StartsWithValidator(string start, bool caseSensitive = true, IValidator<string> validator = null)
            : base(validator)
        {
            this.start = start ?? throw new ArgumentNullException(nameof(start));
            this.caseSensitive = caseSensitive;
        }

        /// <summary>
        /// Validates that a string starts with a specific way. 
        /// <para>A possible nested validator is executed first if one was supplied in the constructor.</para>
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        public override bool IsValid(string value)
        {
            if (!base.IsValid(value))
                return false;

            return value?.StartsWith(start, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase) ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
