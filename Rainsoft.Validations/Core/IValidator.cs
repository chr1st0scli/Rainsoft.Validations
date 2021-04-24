using System.Collections.Generic;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Iterface that defines an abstraction for validating data.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Checks the validity of this instance's data.
        /// </summary>
        /// <returns>True if valid, false otherwise.</returns>
        bool IsValid();

        /// <summary>
        /// Checks the validity of this instance's data.
        /// </summary>
        /// <remarks>
        /// If <paramref name="checkAll"/> is set to false, the check stops at the first offense.
        /// In such a case, <paramref name="offenses"/> contain only one offense if one exists.
        /// </remarks>
        /// <param name="offenses">If the instance is invalid, it contains the validation offenses, otherwise it is empty.</param>
        /// <param name="checkAll">True to gather all validation offenses, false to stop at the first one.</param>
        /// <returns>True if valid, false otherwise.</returns>
        bool IsValid(ref IList<ValidationOffense> offenses, bool checkAll = true);
    }
}
