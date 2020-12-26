using System;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a property that cannot be null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotNullAttribute : Attribute, IObjectValueRule
    {
        public bool IsValid(object value) => value != null;
    }
}
