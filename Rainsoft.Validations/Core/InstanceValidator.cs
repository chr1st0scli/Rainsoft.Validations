using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates a class instance against a set of rules.
    /// </summary>
    /// <typeparam name="T">The instance's type whose members are to be validated.</typeparam>
    public class InstanceValidator<T> : IValidator where T : class
    {
        /// <summary>
        /// The instance that is validated.
        /// </summary>
        protected T instance;

        /// <summary>
        /// A list of the <see cref="instance"/>'s member validators.
        /// </summary>
        protected readonly IList<MemberValidator<T>> memberValidators;

        /// <summary>
        /// Initializes a new <see cref="InstanceValidator{T}"/> for validating a <typeparamref name="T"/> instance.
        /// </summary>
        /// <param name="instance">The instance whose members are to be validated.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="instance"/> is null.</exception>
        public InstanceValidator(T instance)
        {
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            memberValidators = new List<MemberValidator<T>>();
        }

        /// <summary>
        /// Registers a new validator for the instance's member declared in <paramref name="memberExpression"/>.
        /// </summary>
        /// <remarks>The member validator is returned so that successive validation rules can be later attached to it.</remarks>
        /// <param name="memberExpression">An expression that declares the instance's member the validator is created for.</param>
        /// <returns>The new member validator.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="memberExpression"/> is not provided.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="memberExpression"/> is not a valid <see cref="MemberExpression"/>.</exception>
        public MemberValidator<T> Checks(Expression<Func<T, object>> memberExpression)
        {
            var memberValidator = new MemberValidator<T>(instance, memberExpression);

            memberValidators.Add(memberValidator);

            return memberValidator;
        }

        /// <summary>
        /// Runs all validation rules for all registered members of the instance until one fails or all succeed.
        /// </summary>
        /// <returns>True if valid, i.e. if all validations for all registered members have passed, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no validation rules have been added on a registered member.</exception>
        /// <exception cref="InvalidRuleException">Thrown if a member's type is not compatible with a validation rule specified for it.</exception>
        public bool IsValid()
        {
            IList<ValidationOffense> offenses = null;
            return IsValid(ref offenses, false);
        }

        /// <summary>
        /// Runs all validation rules for all registered members of the instance.
        /// </summary>
        /// <param name="offenses">If the instance is invalid, it contains the validation offenses, otherwise it is empty.</param>
        /// <param name="checkAll">True to gather all validation offenses, false to stop at the first one.</param>
        /// <returns>True if valid, i.e. if all validations for all registered members have passed, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no validation rules have been added on a registered member.</exception>
        /// <exception cref="InvalidRuleException">Thrown if a member's type is not compatible with a validation rule specified for it.</exception>
        public bool IsValid(ref IList<ValidationOffense> offenses, bool checkAll = true)
        {
            foreach (var memberValidator in memberValidators)
            {
                bool isValid = memberValidator.IsValid(ref offenses, checkAll);
                if (!isValid && !checkAll)
                    return false;
            }

            return offenses.Count == 0;
        }
    }
}
