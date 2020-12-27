using System;
using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a DateTime property or field that needs to represent a present or past value.
    /// The property's type must be a DateTime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class NotFutureAttribute : Attribute, IObjectValueRule
    {
        private readonly NotFutureValidator _validator;

        /// <summary>
        /// Specifies that the <see cref="DateTime"/> target must belong to the past or present.
        /// </summary>
        public NotFutureAttribute() => _validator = new NotFutureValidator();

        public bool IsValid(object value)
        {
            DateTime dt = this.GetFromDateTime(value);
            return _validator.IsValid(dt);
        }

        public override string ToString() => $"{nameof(NotFutureAttribute)}";
    }
}
