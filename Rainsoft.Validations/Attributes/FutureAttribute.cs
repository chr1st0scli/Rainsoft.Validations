using System;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a DateTime property or field that needs to represent a future value.
    /// The target's type must be a DateTime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class FutureAttribute : Attribute, IObjectValueRule
    {
        private readonly FutureValidator _validator;

        /// <summary>
        /// Specifies that the <see cref="DateTime"/> target must belong to the future.
        /// </summary>
        public FutureAttribute() => _validator = new FutureValidator();

        /// <summary>
        /// Validates that <paramref name="value"/> belongs to the future.
        /// </summary>
        /// <param name="value">The value to validate which must be a DateTime.</param>
        /// <returns>True if value is greater than DateTime.Now.</returns>
        /// <exception cref="InvalidRuleException">Thrown if value is not a DateTime or is null.</exception>
        public bool IsValid(object value)
        {
            DateTime dt = this.GetFromDateTime(value);
            return _validator.IsValid(dt);
        }

        /// <summary>
        /// Returns a string representation of this validation attribute specification.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(FutureAttribute)}";
    }
}
