using System;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that needs to be longer than a given length.
    /// The target's type must be either a string or a primitive type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class LongerThanAttribute : Attribute, IObjectValueRule
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

        public bool IsValid(object value)
        {
            string s = this.GetFromStringOrPrimitive(value);
            return _validator.IsValid(s);
        }

        public override string ToString() => $"{nameof(LongerThanAttribute)} {_length}";
    }
}
