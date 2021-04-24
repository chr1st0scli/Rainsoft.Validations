using System;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that needs to be of a certain length.
    /// The target's type must be either a string or a primitive type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class LengthAttribute : AttributeRule
    {
        private readonly LengthValidator _validator;
        private readonly uint _length;

        /// <summary>
        /// Specifies the desired target's length.
        /// </summary>
        /// <param name="length">The desired length.</param>
        public LengthAttribute(uint length)
        {
            _length = length;
            _validator = new LengthValidator(length);
        }

        /// <summary>
        /// Validates that <paramref name="value"/> is of a certain length.
        /// </summary>
        /// <param name="value">The value to validate. It must be a primitive or string.</param>
        /// <returns>
        /// True if valid or <paramref name="value"/> is null and the intended length is 0.
        /// False if invalid or <paramref name="value"/> is null and the intended length is greater than 0.
        /// </returns>
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
        public override string ToString() => $"{nameof(LengthAttribute)} {_length}";
    }
}
