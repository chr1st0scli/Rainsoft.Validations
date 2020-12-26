using System;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property that needs to be greater than a given margin.
    /// The property's type must be convertible to a double.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
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
