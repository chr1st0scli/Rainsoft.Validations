using System;
using System.Collections.Generic;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property that must belong to a set.
    /// The property's type must be either a string or a primitive type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
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

        public bool IsValid(object value)
        {
            // Just make sure it's a string or a primitive.
            this.GetFromStringOrPrimitive(value);
            return _validator.IsValid(value);
        }

        public override string ToString() => $"{nameof(OneOfAttribute)} {string.Join(",", _values)}";
    }
}
