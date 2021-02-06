using System;
using System.Text.RegularExpressions;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that needs to match with a given regular expression.
    /// Several regular expressions can be combined for a target.
    /// The property's type must be either a string or a primitive type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class MatchesAttribute : Attribute, IObjectValueRule
    {
        private readonly RegexValidator _validator;
        private readonly string _pattern;

        /// <summary>
        /// Specifies the regular expression the target must match with.
        /// </summary>
        /// <param name="pattern">The regular expression which cannot be null.</param>
        /// <param name="options">A bitwise combination options for the regular expression.</param>
        public MatchesAttribute(string pattern, RegexOptions options = RegexOptions.None)
        {
            _pattern = pattern;
            _validator = new RegexValidator(new Regex(pattern, options));
        }

        /// <summary>
        /// Validates that <paramref name="value"/> matches a regular expression.
        /// </summary>
        /// <param name="value">The value to validate. It must be a primitive or string.</param>
        /// <returns>True if valid, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        /// <exception cref="InvalidRuleException">Thrown if value's runtime type is neither a primitive nor a string.</exception>
        public bool IsValid(object value)
        {
            string s = this.GetFromStringOrPrimitive(value);
            return _validator.IsValid(s);
        }

        /// <summary>
        /// Returns a string representation of this validation attribute specification.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(MatchesAttribute)} {_pattern}";
    }
}
