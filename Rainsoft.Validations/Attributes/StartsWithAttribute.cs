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
    public sealed class StartsWithAttribute : Attribute, IObjectValueRule
    {
        private readonly StartsWithValidator _validator;
        private readonly string _start;

        /// <summary>
        /// Specifies the value the target must start with.
        /// </summary>
        /// <param name="start">The starting value which cannot be null.</param>
        /// <param name="caseSensitive">Specifies if start is checked in a case sensitive manner.</param>
        public StartsWithAttribute(string start, bool caseSensitive = true)
        {
            _start = start;
            _validator = new StartsWithValidator(start, caseSensitive);
        }

        public bool IsValid(object value)
        {
            string s = this.GetFromStringOrPrimitive(value);
            return _validator.IsValid(s);
        }

        public override string ToString() => $"{nameof(StartsWithAttribute)} {_start}";
    }
}
