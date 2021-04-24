using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Rainsoft.Validations.MSAnnotations
{
    /// <summary>
    /// Attribute that enables Rainsoft.Validations.Attributes to be used transparently 
    /// with Microsoft's validation infrastructure, i.e. <see cref="System.ComponentModel.DataAnnotations"/>.
    /// </summary>
    /// <remarks>
    /// A potential error message consists of all applicable Rainsoft Attribute error messages.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class MSValidationAdapterAttribute : ValidationAttribute
    {
        /// <summary>
        /// The separator used between potential Rainsoft Attribute error messages. If not specified, a space character is used.
        /// </summary>
        public string ErrorSeparator { get; set; }

        /// <summary>
        /// Uses all Rainsoft validation attributes applied on a member to validate <paramref name="value"/>.
        /// </summary>
        /// <remarks>
        /// A potential error message consists of all applicable Rainsoft Attribute error messages.
        /// </remarks>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of <see cref="ValidationResult"/>.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IList<ValidationOffense> offenses = null;
            if (!validationContext.ObjectInstance.IsMemberValid(validationContext.MemberName, ref offenses))
            {
                var errors = offenses
                    .Select(o => o.ErrorMessage)
                    .Where(s => !string.IsNullOrEmpty(s));

                string errorMessage = string.Join(ErrorSeparator ?? " ", errors);

                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
