using System;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property that needs to start with a specific value.
    /// The property's type must be either a string or a primitive type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class StartsWithAttribute : Attribute, IObjectValueRule
    {
        private readonly StartsWithValidator _validator;
        private readonly string _start;

        /// <summary>
        /// Specifies the value the target must start with.
        /// </summary>
        /// <param name="start">The starting value which cannot be null.</param>
        public StartsWithAttribute(string start)
        {
            _start = start;
            _validator = new StartsWithValidator(start);
        }

        public bool IsValid(object value)
        {
            string s = this.GetFromStringOrPrimitive(value);
            return _validator.IsValid(s);
        }

        public override string ToString() => $"{nameof(StartsWithAttribute)} {_start}";
    }
}
