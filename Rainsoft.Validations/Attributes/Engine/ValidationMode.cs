namespace Rainsoft.Validations.Attributes.Engine
{
    /// <summary>
    /// The mode when validating whole objects.
    /// </summary>
    public enum ValidationMode
    {
        /// <summary>
        /// Only properties are validated based on their declared attributes.
        /// </summary>
        Properties,

        /// <summary>
        /// Only fields are validated based on their declared attributes.
        /// </summary>
        Fields
    }
}
