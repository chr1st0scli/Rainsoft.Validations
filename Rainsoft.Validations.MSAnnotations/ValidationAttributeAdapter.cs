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
    public sealed class ValidationAttributeAdapter : ValidationAttribute
    {
        public ValidationAttributeAdapter(string errorMessage)
            : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IList<ValidationOffense> offenses = new List<ValidationOffense>();
            if (!validationContext.ObjectInstance.IsMemberValid(validationContext.MemberName, ref offenses))
                return new ValidationResult(ErrorMessage, offenses.Select(o => o.MemberName));
            return ValidationResult.Success;
        }
    }
}
