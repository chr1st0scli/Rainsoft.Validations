namespace Rainsoft.Validations.Core
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
        /// The object's property or field name that did not satisfy the rule.
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// The object's property or field value that caused the rule to fail.
        /// </summary>
        public object OffendingValue { get; set; }

        /// <summary>
        /// A string representation of the rule that failed.
        /// </summary>
        public string OffendedRule { get; set; }

        /// <summary>
        /// An error message for the offense.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
