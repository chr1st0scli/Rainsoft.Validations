using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;
using System;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that needs to be less than or equal to a given margin.
    /// The target's type must be convertible to a double.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class LessThanOrEqualToAttribute : AttributeRule
    {
        private readonly LesserValidator<double> _validator;
        private readonly double _margin;

        /// <summary>
        /// Specifies the margin the target must precede or be equal to.
        /// </summary>
        /// <param name="margin">The margin.</param>
        public LessThanOrEqualToAttribute(double margin)
        {
            _margin = margin;
            _validator = new LesserValidator<double>(margin, true);
        }

        /// <summary>
        /// Validates that <paramref name="value"/> is less than or equal to a specified margin.
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
        public override string ToString() => $"{nameof(LessThanOrEqualToAttribute)} {_margin}";
    }
}
