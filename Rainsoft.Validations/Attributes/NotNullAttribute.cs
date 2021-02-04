using Rainsoft.Validations.Attributes.Engine;
using System;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property or field that cannot be null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class NotNullAttribute : Attribute, IObjectValueRule
    {
        public bool IsValid(object value) => value != null;

        public override string ToString() => nameof(NotNullAttribute);
    }
}
