using Rainsoft.Validations.Core;
using System;

namespace Rainsoft.Validations.Attributes.Engine
{
    /// <summary>
    /// Class that must be inherited by concrete validation <see cref="Attribute"/> subclasses.
    /// </summary>
    public abstract class AttributeRule : Attribute, IValueValidator<object>
    {
        /// <summary>
        /// Checks that a value is valid.
        /// </summary>
        /// <remarks>Override this method to implement a concrete validation.</remarks>
        /// <param name="value">The value to be checked.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public abstract bool IsValid(object value);

        /// <summary>
        /// The error message associated with this instance.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
