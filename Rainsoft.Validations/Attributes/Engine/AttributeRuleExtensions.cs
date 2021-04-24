using Rainsoft.Validations.Core;
using System;

namespace Rainsoft.Validations.Attributes.Engine
{
    internal static class AttributeRuleExtensions
    {
        /// <summary>
        /// Gets a string from an object whose runtime type is string or a primitive and throws an exception otherwise.
        /// </summary>
        /// <remarks>If value is null, no check is performed and null is returned.</remarks>
        /// <param name="rule">The object this method is called on.</param>
        /// <param name="value">The object to extract the value from.</param>
        /// <returns>The value as a string.</returns>
        /// <exception cref="InvalidRuleException">Thrown if value's runtime type is neither a primitive nor a string.</exception>
        internal static string GetFromStringOrPrimitive(this AttributeRule rule, object value)
        {
            return ValueUtil.GetFromStringOrPrimitive(rule.GetType(), value);
        }

        /// <summary>
        /// Gets a string from an object whose runtime type is string and throws an exception otherwise.
        /// </summary>
        /// <remarks>If value is null, no check is performed and null is returned.</remarks>
        /// <param name="rule">The object this method is called on.</param>
        /// <param name="value">The object to extract the value from.</param>
        /// <returns>The value as a string.</returns>
        /// <exception cref="InvalidRuleException">Thrown if value's runtime type is not a string.</exception>
        internal static string GetFromString(this AttributeRule rule, object value)
        {
            return ValueUtil.GetFromString(rule.GetType(), value);
        }

        /// <summary>
        /// Gets a double from an object whose runtime type can be converted to one and throws an exception otherwise.
        /// </summary>
        /// <param name="rule">The object this method is called on.</param>
        /// <param name="value">The object to extract the value from.</param>
        /// <returns>The value as a double.</returns>
        /// <exception cref="InvalidRuleException">Thrown if value is null or its runtime type is not compatible with a double.</exception>
        internal static double GetFromDoubleCompatible(this AttributeRule rule, object value)
        {
            return ValueUtil.GetFromDoubleCompatible(rule.GetType(), value);
        }

        /// <summary>
        /// Gets a DateTime from an object and an exception is thrown if the boxed value is not a DateTime.
        /// </summary>
        /// <param name="rule">The object this method is called on.</param>
        /// <param name="value">The object to extract the value from.</param>
        /// <returns>The value as a DateTime.</returns>
        /// <exception cref="InvalidRuleException">Thrown if value's runtime type is not DateTime.</exception>
        internal static DateTime GetFromDateTime(this AttributeRule rule, object value)
        {
            return ValueUtil.GetFromDateTime(rule.GetType(), value);
        }
    }
}
