using System;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that needs to be less than a given margin.
    /// The target's type must be convertible to a double.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class LessThanAttribute : AttributeRule
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

        /// <summary>
        /// Validates that <paramref name="value"/> is less than a specified margin.
        /// </summary>
        /// <param name="value">The value to validate which must be convertible to a double.</param>
        /// <returns>True if valid, false otherwise.</returns>
        /// <exception cref="InvalidRuleException">Thrown if value is null or its runtime type is not compatible with a double.</exception>
        public override bool IsValid(object value)
        {
            double d = this.GetFromDoubleCompatible(value);
            return _validator.IsValid(d);
        }

        /// <summary>
        /// Returns a string representation of this validation attribute specification.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(LessThanAttribute)} {_margin}";
    }
}
