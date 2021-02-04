using System;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that needs to be shorter than a given length.
    /// The target's type must be either a string or a primitive type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ShorterThanAttribute : Attribute, IObjectValueRule
    {
        private readonly ShorterValidator _validator;
        private readonly uint _length;

        /// <summary>
        /// Specifies the length the target must not exceed.
        /// </summary>
        /// <param name="length">The length that must not be exceeded.</param>
        public ShorterThanAttribute(uint length)
        {
            _length = length;
            _validator = new ShorterValidator(length);
        }

        public bool IsValid(object value)
        {
            string s = this.GetFromStringOrPrimitive(value);
            return _validator.IsValid(s);
        }

        public override string ToString() => $"{nameof(ShorterThanAttribute)} {_length}";
    }
}
