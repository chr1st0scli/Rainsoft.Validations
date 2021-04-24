using System;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that needs to be longer than a given length.
    /// The target's type must be either a string or a primitive type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class LongerThanAttribute : AttributeRule
    {
        private readonly LongerValidator _validator;
        private readonly uint _length;

        /// <summary>
        /// Specifies the length the target must exceed.
        /// </summary>
        /// <param name="length">The length that must be exceeded.</param>
        public LongerThanAttribute(uint length)
        {
            _length = length;
            _validator = new LongerValidator(length);
        }

        /// <summary>
        /// Validates that <paramref name="value"/>'s length is greater than a given number.
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
        public override string ToString() => $"{nameof(LongerThanAttribute)} {_length}";
    }
}
