using System;
using System.Text.RegularExpressions;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given string satisfies a regular expression.
    /// </summary>
    public class RegexValidator : ValidatorDecorator<string>
    {
        /// <summary>
        /// The regular expression used for validating a value.
        /// </summary>
        protected Regex regex;

        /// <summary>
        /// Constructs a validator for checking a value's pattern.
        /// </summary>
        /// <param name="regex">The regular expression to use.</param>
        /// <param name="validator">A string validator that can be combined with this one.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="regex"/> is null.</exception>
        public RegexValidator(Regex regex, IValueValidator<string> validator = null)
            : base(validator)
        {
            this.regex = regex ?? throw new ArgumentNullException(nameof(regex));
        }

        /// <summary>
        /// Validates that <paramref name="value"/> matches a regular expression.
        /// <para>A possible nested validator is executed first if one was supplied in the constructor.</para>
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if valid, false otherwise or if <paramref name="value"/> is null.</returns>
        public override bool IsValid(string value)
        {
            if (!base.IsValid(value) || value == null)
                return false;

            return regex.IsMatch(value);
        }

        /// <summary>
        /// Returns a string representation of this validator.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(RegexValidator)} {regex}";
    }
}
