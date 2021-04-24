namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Decorator that allows combining different validators in a nested fashion.
    /// Inherit from this class to make a validator combinable with another.
    /// </summary>
    /// <typeparam name="T">The type of data to be validated.</typeparam>
    public class ValidatorDecorator<T> : IValueValidator<T>
    {
        private readonly IValueValidator<T> _validator;

        /// <summary>
        /// Constructs a nested validator decorator.
        /// </summary>
        /// <param name="validator">The nested validator. Supply null to terminate the validator nesting.</param>
        public ValidatorDecorator(IValueValidator<T> validator) => _validator = validator;

        /// <summary>
        /// Validates a value using the nested validator if specified in the constructor. If not specified, it returns true. 
        /// Override this method to specify a validation and call this one first to support nesting validators.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if value is valid or no validator was specified, false otherwise.</returns>
        public virtual bool IsValid(T value)
            => _validator?.IsValid(value) ?? true;
    }
}
