using Rainsoft.Validations.Attributes.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Rainsoft.Validations.MSAnnotations
{
    /// <summary>
    /// Attribute that enables Rainsoft.Validations.Attributes to be used transparently 
    /// with Microsoft's validation infrastructure, i.e. System.ComponentModel.DataAnnotations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class MSValidationAdapterAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes an instance of the <see cref="MSValidationAdapterAttribute"/> class 
        /// with a specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public MSValidationAdapterAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        /// <summary>
        /// Uses all possible Rainsoft validation attributes applied on a member to validate <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of <see cref="ValidationResult"/>.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IList<ValidationOffense> offenses = new List<ValidationOffense>();
            if (!validationContext.ObjectInstance.IsMemberValid(validationContext.MemberName, ref offenses))
                return new ValidationResult(ErrorMessage, offenses.Select(o => o.MemberName));

            return ValidationResult.Success;
        }
    }
}
