using System;

namespace Rainsoft.Validations.Attributes
{
    internal static class IObjectValueRuleExtensions
    {
        /// <summary>
        /// Gets a string from an object whose runtime type is string or a primitive and throws an exception otherwise.
        /// If value is null, no check is performed and an empty string is returned.
        /// </summary>
        /// <param name="rule">The object this method is called on.</param>
        /// <param name="value">The object to extract the value from.</param>
        /// <returns>The value as a string.</returns>
        /// <exception cref="InvalidRuleException">Thrown if value's runtime type is neither a primitive nor a string.</exception>
        internal static string GetFromStringOrPrimitive(this IObjectValueRule rule, object value)
        {
            Type t = value?.GetType();
            if (t != null && !t.IsPrimitive && t != typeof(string))
                throw new InvalidRuleException { RuleType = rule.GetType(), TargetType = t };

            return Convert.ToString(value);
        }

        /// <summary>
        /// Gets a string from an object whose runtime type is string and throws an exception otherwise.
        /// If value is null, no check is performed and an empty string is returned.
        /// </summary>
        /// <param name="rule">The object this method is called on.</param>
        /// <param name="value">The object to extract the value from.</param>
        /// <returns>The value as a string.</returns>
        /// <exception cref="InvalidRuleException">Thrown if value's runtime type is not a string.</exception>
        internal static string GetFromString(this IObjectValueRule rule, object value)
        {
            Type t = value?.GetType();
            if (t != null && t != typeof(string))
                throw new InvalidRuleException { RuleType = rule.GetType(), TargetType = t };

            return Convert.ToString(value);
        }

        /// <summary>
        /// Gets a double from an object whose runtime type can be converted to one and throws an exception otherwise.
        /// </summary>
        /// <param name="rule">The object this method is called on.</param>
        /// <param name="value">The object to extract the value from.</param>
        /// <returns>The value as a double.</returns>
        /// <exception cref="InvalidRuleException">Thrown if value's runtime type is not compatible with a double.</exception>
        internal static double GetFromDoubleCompatible(this IObjectValueRule rule, object value)
        {
            try
            {
                // Do not use a cast, because unboxing an int as a double would throw an InvalidCastException.
                // Don't accept null either, because this would be converted to 0 and result in wrong comparisons.
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                return Convert.ToDouble(value);
            }
            catch (Exception ex)
            {
                throw new InvalidRuleException(ex.Message, ex)
                {
                    RuleType = rule.GetType(),
                    TargetType = value?.GetType()
                };
            }
        }

        /// <summary>
        /// Gets a DateTime from an object and an exception is thrown if the boxed value is not a DateTime.
        /// </summary>
        /// <param name="rule">The object this method is called on.</param>
        /// <param name="value">The object to extract the value from.</param>
        /// <returns>The value as a DateTime.</returns>
        /// <exception cref="InvalidRuleException">Thrown if value's runtime type is not DateTime.</exception>
        internal static DateTime GetFromDateTime(this IObjectValueRule rule, object value)
        {
            try
            {
                // Use a cast because we expect nothing else but a DateTime.
                return (DateTime)value;
            }
            catch (Exception ex)
            {
                throw new InvalidRuleException(ex.Message, ex)
                {
                    RuleType = rule.GetType(),
                    TargetType = value?.GetType()
                };
            }
        }
    }
}
