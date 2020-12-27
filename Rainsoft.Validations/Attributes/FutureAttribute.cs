using System;
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

        public bool IsValid(object value)
        {
            DateTime dt = this.GetFromDateTime(value);
            return _validator.IsValid(dt);
        }

        public override string ToString() => $"{nameof(FutureAttribute)}";
    }
}
