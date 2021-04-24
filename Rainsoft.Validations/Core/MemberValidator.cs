using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates a class's accessible member against a collection of rules.
    /// </summary>
    /// <typeparam name="T">The instance's type whose member is to be validated.</typeparam>
    public class MemberValidator<T> : IValidator where T : class
    {
        /// <summary>
        /// The instance that is validated.
        /// </summary>
        protected T instance;

        /// <summary>
        /// The instance's member name that is validated.
        /// </summary>
        protected string memberName;

        /// <summary>
        /// An expression that returns the member's value.
        /// </summary>
        protected Expression<Func<T, object>> memberExpression;

        /// <summary>
        /// Defines a validation rule callback.
        /// </summary>
        /// <param name="value">The member's value to validate.</param>
        /// <param name="offenses">The collection that a possible offense of this rule, will be added to.</param>
        /// <returns>True if valid, false otherwise.</returns>
        protected delegate bool ValidationCall(object value, IList<ValidationOffense> offenses);

        /// <summary>
        /// The list of validation rule callbacks to be applied on the member's value.
        /// </summary>
        protected IList<ValidationCall> validationCalls;

        /// <summary>
        /// Initializes an instance of <see cref="MemberValidator{T}"/> that validates a class's accessible member against a collection of rules.
        /// </summary>
        /// <param name="instance">The instance whose member is to be validated.</param>
        /// <param name="memberExpression">An expression that declares the member.</param>
        /// <exception cref="ArgumentNullException">Thrown if an instance or a member expression is not provided.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="memberExpression"/> is not a valid <see cref="MemberExpression"/>.</exception>
        public MemberValidator(T instance, Expression<Func<T, object>> memberExpression)
        {
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.memberExpression = memberExpression ?? throw new ArgumentNullException(nameof(memberExpression));

            if (memberExpression.Body is MemberExpression mexpr)
                memberName = mexpr.Member.Name;
            else if (memberExpression.Body is UnaryExpression uexpr && uexpr.Operand is MemberExpression innerMemberExpr)
                memberName = innerMemberExpr.Member.Name;
            else
                throw new ArgumentException($"A {nameof(MemberExpression)} should be used.", nameof(memberExpression));
            
            validationCalls = new List<ValidationCall>();
        }

        /// <summary>
        /// Adds a deferred validation call to <see cref="validationCalls"/> for the member's <see cref="string"/> or primitive value.
        /// </summary>
        /// <remarks>
        /// If <paramref name="strictlyString"/> is set to true, the deferred validation call will try to extract
        /// the member's value from a string. Otherwise, it will try to do so from a primitive type as well.
        /// </remarks>
        /// <param name="validator">The string validator that implements the validation rule.</param>
        /// <param name="strictlyString">If true, the member's type is expected to be a <see cref="string"/>. Default is false.</param>
        /// <param name="errorMessage">An optional error message to be associated with a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further validation calls.</returns>
        protected MemberValidator<T> AddValidation(IValueValidator<string> validator, bool strictlyString = false, string errorMessage = null)
        {
            validationCalls.Add((value, offenses) =>
            {
                string s = strictlyString
                    ? ValueUtil.GetFromString(validator.GetType(), value)
                    : ValueUtil.GetFromStringOrPrimitive(validator.GetType(), value);
                return Validate(validator, s, offenses, errorMessage);
            });
            return this;
        }

        /// <summary>
        /// Adds a deferred validation call to <see cref="validationCalls"/> for the member's value that is compatible with a <see cref="double"/>.
        /// </summary>
        /// <param name="validator">The double validator that implements the validation rule.</param>
        /// <param name="errorMessage">An optional error message to be associated with a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further validation calls.</returns>
        protected MemberValidator<T> AddValidation(IValueValidator<IComparable<double>> validator, string errorMessage = null)
        {
            validationCalls.Add((value, offenses) =>
            {
                double d = ValueUtil.GetFromDoubleCompatible(validator.GetType(), value);
                return Validate(validator, d, offenses, errorMessage);
            });
            return this;
        }

        /// <summary>
        /// Adds a deferred validation call to <see cref="validationCalls"/> for the member's <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="validator">The DateTime validator that implements the validation rule.</param>
        /// <param name="errorMessage">An optional error message to be associated with a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further validation calls.</returns>
        protected MemberValidator<T> AddValidation(IValueValidator<DateTime> validator, string errorMessage = null)
        {
            validationCalls.Add((value, offenses) =>
            {
                DateTime dt = ValueUtil.GetFromDateTime(validator.GetType(), value);
                return Validate(validator, dt, offenses, errorMessage);
            });
            return this;
        }

        /// <summary>
        /// Adds a deferred validation call to <see cref="validationCalls"/> for the member's <see cref="object"/> value.
        /// </summary>
        /// <remarks>
        /// If <paramref name="strictlyStringOrPrimitive"/> is set to true, the deferred validation call 
        /// will make sure that the member's type is either a string or primitive.
        /// </remarks>
        /// <param name="validator">The object validator that implements the validation rule.</param>
        /// <param name="strictlyStringOrPrimitive">If true, the member's type is expected to be a <see cref="string"/> or primitive. Default is false.</param>
        /// <param name="errorMessage">An optional error message to be associated with a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further validation calls.</returns>
        protected MemberValidator<T> AddValidation(IValueValidator<object> validator, bool strictlyStringOrPrimitive = false, string errorMessage = null)
        {
            validationCalls.Add((value, offenses) =>
            {
                if (strictlyStringOrPrimitive)
                    ValueUtil.GetFromStringOrPrimitive(validator.GetType(), value);
                return Validate(validator, value, offenses, errorMessage);
            });
            return this;
        }

        /// <summary>
        /// The core implementation of a deferred validation rule based on an <see cref="IValueValidator{T}"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to be validated.</typeparam>
        /// <param name="validator">The validator that implements the validation rule.</param>
        /// <param name="value">The value to validate.</param>
        /// <param name="offenses">The collection that a possible offense of this rule, will be added to.</param>
        /// <param name="errorMessage">An error message to be associated with a potential validation offense.</param>
        /// <returns>True if <paramref name="value"/> is valid, false otherwise.</returns>
        protected bool Validate<TValue>(IValueValidator<TValue> validator, TValue value, IList<ValidationOffense> offenses, string errorMessage)
        {
            bool isValid = validator.IsValid(value);
            if (!isValid)
                offenses.Add(MakeOffense(value, validator.ToString(), errorMessage));
            return isValid;
        }

        /// <summary>
        /// Creates a new <see cref="ValidationOffense"/> instance for this member.
        /// </summary>
        /// <param name="value">The offending value.</param>
        /// <param name="rule">The rule's description that failed.</param>
        /// <param name="errorMessage">An error message to associate with the offense.</param>
        /// <returns>A new offense instance.</returns>
        protected ValidationOffense MakeOffense(object value, string rule, string errorMessage)
            => new ValidationOffense
            {
                TypeName = instance.GetType().Name,
                MemberName = memberName,
                OffendingValue = value,
                OffendedRule = rule,
                ErrorMessage = errorMessage
            };

        /// <summary>
        /// Runs all deferred validation rules that have been registered for the member's value until one fails or all succeed.
        /// </summary>
        /// <returns>True if valid, i.e. if all validations have passed, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no validation rules have been added on the member.</exception>
        /// <exception cref="InvalidRuleException">Thrown if the member's type is not compatible with a validation rule specified for it.</exception>
        public bool IsValid()
        {
            IList<ValidationOffense> offenses = null;
            return IsValid(ref offenses, false);
        }

        /// <summary>
        /// Runs all deferred validation rules that have been registered for the member's value.
        /// </summary>
        /// <param name="offenses">If the instance is invalid, it contains the validation offenses, otherwise it is empty.</param>
        /// <param name="checkAll">True to gather all validation offenses, false to stop at the first one.</param>
        /// <returns>True if valid, i.e. if all validations have passed, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no validation rules have been added on the member.</exception>
        /// <exception cref="InvalidRuleException">Thrown if the member's type is not compatible with a validation rule specified for it.</exception>
        public bool IsValid(ref IList<ValidationOffense> offenses, bool checkAll = true)
        {
            if (validationCalls.Count == 0)
                throw new InvalidOperationException("No rules have been defined for this member validator.");

            if (offenses == null)
                offenses = new List<ValidationOffense>();

            var memberValue = memberExpression.Compile()(instance);

            foreach (var isValid in validationCalls)
            {
                if (!isValid(memberValue, offenses) && !checkAll)
                    return false;
            }

            return offenses.Count == 0;
        }

        /// <summary>
        /// Adds a deferred rule which checks that the member's string or primitive value starts with a specific value.
        /// </summary>
        /// <param name="start">The desired way the value must start with.</param>
        /// <param name="caseSensitive">Specifies if <paramref name="start"/> is checked in a case sensitive manner.</param>
        /// <param name="errorMessage">An optional error message for a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further rules.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="start"/> is null.</exception>
        public MemberValidator<T> StartsWith(string start, bool caseSensitive = true, string errorMessage = null)
            => AddValidation(new StartsWithValidator(start, caseSensitive), errorMessage: errorMessage);

        /// <summary>
        /// Adds a deferred rule which checks that the member's string or primitive value ends with a specific value.
        /// </summary>
        /// <param name="end">The desired way the value must end with.</param>
        /// <param name="caseSensitive">Specifies if <paramref name="end"/> is checked in a case sensitive manner.</param>
        /// <param name="errorMessage">An optional error message for a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further rules.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="end"/> is null.</exception>
        public MemberValidator<T> EndsWith(string end, bool caseSensitive = true, string errorMessage = null)
            => AddValidation(new EndsWithValidator(end, caseSensitive), errorMessage: errorMessage);

        /// <summary>
        /// Adds a deferred rule which checks that the member's string or primitive value is of a certain length.
        /// </summary>
        /// <param name="length">The desired value's length.</param>
        /// <param name="errorMessage">An optional error message for a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further rules.</returns>
        public MemberValidator<T> OfLength(uint length, string errorMessage = null)
            => AddValidation(new LengthValidator(length), errorMessage: errorMessage);

        /// <summary>
        /// Adds a deferred rule which checks that the member's string or primitive value is longer than a certain length.
        /// </summary>
        /// <param name="length">The length the value's length must surpass.</param>
        /// <param name="errorMessage">An optional error message for a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further rules.</returns>
        public MemberValidator<T> LongerThan(uint length, string errorMessage = null)
            => AddValidation(new LongerValidator(length), errorMessage: errorMessage);

        /// <summary>
        /// Adds a deferred rule which checks that the member's string or primitive value is shorter than a certain length.
        /// </summary>
        /// <param name="length">The length that must surpass the value's length.</param>
        /// <param name="errorMessage">An optional error message for a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further rules.</returns>
        public MemberValidator<T> ShorterThan(uint length, string errorMessage = null)
            => AddValidation(new ShorterValidator(length), errorMessage: errorMessage);

        /// <summary>
        /// Adds a deferred rule which checks that the member's string or primitive value matches a regular expression.
        /// </summary>
        /// <param name="regex">The regular expression to use.</param>
        /// <param name="errorMessage">An optional error message for a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further rules.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="regex"/> is null.</exception>
        public MemberValidator<T> Matches(Regex regex, string errorMessage = null)
            => AddValidation(new RegexValidator(regex), errorMessage: errorMessage);

        /// <summary>
        /// Adds a deferred rule which checks that the member's double-compatible value is greater than, or optionally equal to, a margin.
        /// </summary>
        /// <param name="margin">The margin that the value must surpass or optionally be equal to.</param>
        /// <param name="orEqualTo">If true, it is checked that the value is greater than or equal to <paramref name="margin"/>. If false, it is checked that the value is only greater than <paramref name="margin"/>.</param>
        /// <param name="errorMessage">An optional error message for a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further rules.</returns>
        public MemberValidator<T> GreaterThan(double margin, bool orEqualTo = false, string errorMessage = null)
            => AddValidation(new GreaterValidator<double>(margin, orEqualTo), errorMessage);

        /// <summary>
        /// Adds a deferred rule which checks that the member's double-compatible value is less than, or optionally equal to, a margin.
        /// </summary>
        /// <param name="margin">The margin that the value must be less than or optionally equal to.</param>
        /// <param name="orEqualTo">If true, it is checked that the value is less than or equal to <paramref name="margin"/>. If false, it is checked that the value is only less than <paramref name="margin"/>.</param>
        /// <param name="errorMessage">An optional error message for a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further rules.</returns>
        public MemberValidator<T> LessThan(double margin, bool orEqualTo = false, string errorMessage = null)
            => AddValidation(new LesserValidator<double>(margin, orEqualTo), errorMessage);

        /// <summary>
        /// Adds a deferred rule which checks that the member's string or primitive value belongs to a given set.
        /// </summary>
        /// <param name="values">The set the value must belong to.</param>
        /// <param name="errorMessage">An optional error message for a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further rules.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is null.</exception>
        public MemberValidator<T> OneOf(IEnumerable<object> values, string errorMessage = null)
            => AddValidation(new OneOfValidator<object>(values), true, errorMessage);

        /// <summary>
        /// Adds a deferred rule which checks that the member's <see cref="DateTime"/> value does not belong to the future.
        /// </summary>
        /// <param name="errorMessage">An optional error message for a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further rules.</returns>
        public MemberValidator<T> NotInFuture(string errorMessage = null)
            => AddValidation(new NotFutureValidator(), errorMessage);

        /// <summary>
        /// Adds a deferred rule which checks that the member's <see cref="DateTime"/> value belongs to the future.
        /// </summary>
        /// <param name="errorMessage">An optional error message for a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further rules.</returns>
        public MemberValidator<T> InFuture(string errorMessage = null)
            => AddValidation(new FutureValidator(), errorMessage);

        /// <summary>
        /// Adds a deferred rule which checks that the member's value is not null.
        /// </summary>
        /// <param name="errorMessage">An optional error message for a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further rules.</returns>
        public MemberValidator<T> NotNull(string errorMessage = null)
        {
            var validator = new PredicateValidator<object>(obj => obj is object) 
            { 
                PredicateDescription = nameof(NotNull)
            };

            return AddValidation(validator, errorMessage: errorMessage);
        }

        /// <summary>
        /// Adds a deferred rule which checks that the member's <see cref="string"/> value is not null or comprised of just whitespace characters.
        /// </summary>
        /// <param name="errorMessage">An optional error message for a potential validation offense.</param>
        /// <returns>This instance to expressibly chain further rules.</returns>
        public MemberValidator<T> NotNullOrWhiteSpace(string errorMessage = null)
        {
            var validator = new PredicateValidator<string>(s => s?.Trim().Length > 0)
            {
                PredicateDescription = nameof(NotNullOrWhiteSpace)
            };

            return AddValidation(validator, true, errorMessage);
        }
    }
}
