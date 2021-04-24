using System;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a value satisfies a predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value passed to the predicate.</typeparam>
    public class PredicateValidator<T> : ValidatorDecorator<T>
    {
        /// <summary>
        /// A method that returns true for valid and false otherwise.
        /// </summary>
        protected Predicate<T> predicate;

        /// <summary>
        /// Constructs a validator that validates using a predicate.
        /// </summary>
        /// <param name="predicate">A method that returns true for valid and false otherwise.</param>
        /// <param name="validator">A validator of the same type that can be combined with this one.</param>
        /// <exception cref="ArgumentNullException">Thrown if predicate is null.</exception>
        public PredicateValidator(Predicate<T> predicate, IValueValidator<T> validator = null)
            : base(validator)
        {
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        /// <summary>
        /// Validates that value is valid according to a predicate. 
        /// <para>A possible nested validator is executed first if one was supplied in the constructor.</para>
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public override bool IsValid(T value)
        {
            if (!base.IsValid(value))
                return false;

            return predicate(value);
        }

        /// <summary>
        /// Describes the <see cref="predicate"/>. It can be set to affect the <see cref="ToString"/> method's return value.
        /// </summary>
        public string PredicateDescription { get; set; }

        /// <summary>
        /// Returns a string representation of this validator.
        /// </summary>
        /// <remarks>
        /// <see cref="PredicateDescription"/> is used, if not null or whitespace. Otherwise, the name of this instance's type is used.
        /// </remarks>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(PredicateDescription))
                return PredicateDescription;

            return $"{nameof(PredicateValidator<T>)}";
        }
    }
}
