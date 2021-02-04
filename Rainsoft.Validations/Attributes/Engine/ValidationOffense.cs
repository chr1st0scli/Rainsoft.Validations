namespace Rainsoft.Validations.Attributes.Engine
{
    /// <summary>
    /// The information for a validation rule that failed, i.e. it is not satisfied.
    /// </summary>
    public class ValidationOffense
    {
        /// <summary>
        /// The runtime type name of the object the rule ran on.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// The actual rule that failed.
        /// </summary>
        public IObjectValueRule Rule { get; set; }

        /// <summary>
        /// The object's property or field name that did not satisfy the rule.
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// The object's property value that caused the rule to fail.
        /// </summary>
        public object OffendingValue { get; set; }
    }
}
