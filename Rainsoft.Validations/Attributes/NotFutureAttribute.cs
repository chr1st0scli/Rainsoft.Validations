using System;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a DateTime property or field that needs to represent a present or past value.
    /// The property's type must be a DateTime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class NotFutureAttribute : AttributeRule
    {
        private readonly NotFutureValidator _validator;

        /// <summary>
        /// Specifies that the <see cref="DateTime"/> target must belong to the past or present.
        /// </summary>
        public NotFutureAttribute() => _validator = new NotFutureValidator();

        /// <summary>
        /// Validates that <paramref name="value"/> does not belong to the future.
        /// </summary>
        /// <param name="value">The value to validate which must be a DateTime.</param>
        /// <returns>True if <paramref name="value"/> is less than or equal to DateTime.Now.</returns>
        /// <exception cref="InvalidRuleException">Thrown if value is not a DateTime or is null.</exception>
        public override bool IsValid(object value)
        {
            DateTime dt = this.GetFromDateTime(value);
            return _validator.IsValid(dt);
        }

        /// <summary>
        /// Returns a string representation of this validation attribute specification.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(NotFutureAttribute)}";
    }
}
