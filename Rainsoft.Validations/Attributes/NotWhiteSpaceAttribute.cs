using System;

namespace Rainsoft.Validations.Attributes
{
    /// <summary>
    /// Attribute for declaring a string property that cannot be 
    /// comprised of just whitespace characters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotWhiteSpaceAttribute : Attribute, IObjectValueRule
    {
        public bool IsValid(object value)
        {
            string s = this.GetFromString(value);
            return s.Trim().Length > 0;
        }
    }
}
