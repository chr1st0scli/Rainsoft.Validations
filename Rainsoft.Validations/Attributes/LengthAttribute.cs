using System;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property that needs to be of a certain length.
    /// The property's type must be either a string or a primitive type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class LengthAttribute : Attribute, IObjectValueRule
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

        public bool IsValid(object value)
        {
            string s = this.GetFromStringOrPrimitive(value);
            return _validator.IsValid(s);
        }

        public override string ToString() => $"{nameof(LengthAttribute)} {_length}";
    }
}
