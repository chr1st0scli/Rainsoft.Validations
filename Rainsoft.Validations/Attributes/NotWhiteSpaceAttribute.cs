using Rainsoft.Validations.Attributes.Engine;
using System;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a string property or field that cannot be comprised of just whitespace characters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class NotWhiteSpaceAttribute : Attribute, IObjectValueRule
    {
        /// <summary>
        /// Validates that <paramref name="value"/> is not comprised of just whitespace characters.
        /// </summary>
        /// <param name="value">The value to validate. It must be a string.</param>
        /// <returns>True if valid, false otherwise or if value is null or an empty string.</returns>
        /// <exception cref="InvalidRuleException">Thrown if value's runtime type is not a string.</exception>
        public bool IsValid(object value)
        {
            string s = this.GetFromString(value);
            return s.Trim().Length > 0;
        }

        /// <summary>
        /// Returns a string representation of this validation attribute specification.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => nameof(NotWhiteSpaceAttribute);
    }
}
