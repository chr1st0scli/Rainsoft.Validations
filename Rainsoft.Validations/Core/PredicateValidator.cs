using System;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a value satisfies a predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value passed to the predicate.</typeparam>
    public class PredicateValidator<T> : ValidatorDecorator<T>
    {
        protected Predicate<T> predicate;

        /// <summary>
        /// Constructs a validator that validates using a predicate.
        /// </summary>
        /// <param name="predicate">A method that returns true for valid and false otherwise.</param>
        /// <param name="validator">A validator of the same type that can be combined with this one.</param>
        /// <exception cref="ArgumentNullException">Thrown if predicate is null.</exception>
        public PredicateValidator(Predicate<T> predicate, IValidator<T> validator = null)
            : base(validator)
        {
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        /// <summary>
        /// Validates that value is valid according to a predicate. 
        /// A possible nested validator is executed first if one was supplied in the constructor.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        public override bool IsValid(T value)
        {
            if (!base.IsValid(value))
                return false;
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            return predicate(value);
        }
    }
}
