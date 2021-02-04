using System;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that needs to be greater than a given margin.
    /// The target's type must be convertible to a double.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class GreaterThanAttribute : Attribute, IObjectValueRule
    {
        private readonly GreaterValidator<double> _validator;
        private readonly double _margin;

        /// <summary>
        /// Specifies the margin the target must supersede.
        /// </summary>
        /// <param name="margin">The margin.</param>
        public GreaterThanAttribute(double margin)
        {
            _margin = margin;
            _validator = new GreaterValidator<double>(margin);
        }

        public bool IsValid(object value)
        {
            double d = this.GetFromDoubleCompatible(value);
            return _validator.IsValid(d);
        }

        public override string ToString() => $"{nameof(GreaterThanAttribute)} {_margin}";
    }
}
