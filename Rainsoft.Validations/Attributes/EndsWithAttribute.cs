using System;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that needs to end with a specific value.
    /// The target's type must be either a string or a primitive type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class EndsWithAttribute : Attribute, IObjectValueRule
    {
        private readonly EndsWithValidator _validator;
        private readonly string _end;

        /// <summary>
        /// Specifies the value the target must end with.
        /// </summary>
        /// <param name="end">The ending value which cannot be null.</param>
        /// <param name="caseSensitive">Specifies if end is checked in a case sensitive manner.</param>
        public EndsWithAttribute(string end, bool caseSensitive = true)
        {
            _end = end;
            _validator = new EndsWithValidator(end, caseSensitive);
        }

        /// <summary>
        /// Validates that <paramref name="value"/> ends with a specific way.
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
        public override string ToString() => $"{nameof(EndsWithAttribute)} {_end}";
    }
}
