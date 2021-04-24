using Rainsoft.Validations.Attributes.Engine;
using System;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that cannot be null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class NotNullAttribute : AttributeRule
    {
        /// <summary>
        /// Validates that <paramref name="value"/> is not null.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public override bool IsValid(object value) => value is object;

        /// <summary>
        /// Returns a string representation of this validation attribute specification.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => nameof(NotNullAttribute);
    }
}
