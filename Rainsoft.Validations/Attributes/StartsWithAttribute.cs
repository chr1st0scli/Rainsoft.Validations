using System;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that needs to start with a specific value.
    /// The target's type must be either a string or a primitive type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class StartsWithAttribute : AttributeRule
    {
        private readonly StartsWithValidator _validator;
        private readonly string _start;

        /// <summary>
        /// Specifies the value the target must start with.
        /// </summary>
        /// <param name="start">The starting value which cannot be null.</param>
        /// <param name="caseSensitive">Specifies if start is checked in a case sensitive manner.</param>
        /// <exception cref="ArgumentNullException">Thrown if start is null.</exception>
        public StartsWithAttribute(string start, bool caseSensitive = true)
        {
            _start = start;
            _validator = new StartsWithValidator(start, caseSensitive);
        }

        /// <summary>
        /// Validates that <paramref name="value"/> starts with a specific way.
        /// </summary>
        /// <param name="value">The value to validate. It must be a primitive or string.</param>
        /// <returns>True if valid, false otherwise or if <paramref name="value"/> is null.</returns>
        /// <exception cref="InvalidRuleException">Thrown if value's runtime type is neither a primitive nor a string.</exception>
        public override bool IsValid(object value)
        {
            string s = this.GetFromStringOrPrimitive(value);
            return _validator.IsValid(s);
        }

        /// <summary>
        /// Returns a string representation of this validation attribute specification.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(StartsWithAttribute)} {_start}";
    }
}
