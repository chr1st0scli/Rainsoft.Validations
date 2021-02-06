using System;
using System.Collections.Generic;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that must belong to a set.
    /// The target's type must be either a string or a primitive type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class OneOfAttribute : Attribute, IObjectValueRule
    {
        private readonly OneOfValidator<object> _validator;
        private readonly IEnumerable<object> _values;

        /// <summary>
        /// Specifies the values that make up the set the target must belong to.
        /// </summary>
        /// <param name="values">The values.</param>
        public OneOfAttribute(params object[] values)
        {
            _values = values;
            _validator = new OneOfValidator<object>(values);
        }

        /// <summary>
        /// Validates that <paramref name="value"/> belongs to a certain set.
        /// </summary>
        /// <param name="value">The value to be searched for. It must be a primitive or string.</param>
        /// <returns>True if valid, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        /// <exception cref="InvalidRuleException">Thrown if value's runtime type is neither a primitive nor a string.</exception>
        public bool IsValid(object value)
        {
            // Just make sure it's a string or a primitive.
            this.GetFromStringOrPrimitive(value);
            return _validator.IsValid(value);
        }

        /// <summary>
        /// Returns a string representation of this validation attribute specification.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString() => $"{nameof(OneOfAttribute)} {string.Join(",", _values)}";
    }
}
