using System;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that needs to be less than a given margin.
    /// The target's type must be convertible to a double.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class LessThanAttribute : Attribute, IObjectValueRule
    {
        private readonly LesserValidator<double> _validator;
        private readonly double _margin;

        /// <summary>
        /// Specifies the margin the target must precede.
        /// </summary>
        /// <param name="margin">The margin.</param>
        public LessThanAttribute(double margin)
        {
            _margin = margin;
            _validator = new LesserValidator<double>(margin);
        }

        public bool IsValid(object value)
        {
            double d = this.GetFromDoubleCompatible(value);
            return _validator.IsValid(d);
        }

        public override string ToString() => $"{nameof(LessThanAttribute)} {_margin}";
    }
}
