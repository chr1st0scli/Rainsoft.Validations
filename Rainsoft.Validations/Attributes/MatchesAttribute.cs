using System;
using System.Text.RegularExpressions;
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

        public bool IsValid(object value)
        {
            string s = this.GetFromStringOrPrimitive(value);
            return _validator.IsValid(s);
        }

        public override string ToString() => $"{nameof(MatchesAttribute)} {_pattern}";
    }
}
