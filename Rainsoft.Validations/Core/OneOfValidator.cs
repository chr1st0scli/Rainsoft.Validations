using System;
using System.Collections.Generic;
using System.Linq;

namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Validates that a given value belongs to a set of acceptable values.
    /// </summary>
    /// <typeparam name="T">The type of a value to be searched in a set of acceptable values.</typeparam>
    public class OneOfValidator<T> : ValidatorDecorator<T>
    {
        /// <summary>
        /// The set a value must belong to.
        /// </summary>
        protected IEnumerable<T> values;

        /// <summary>
        /// Comparer to be used when searching in <see cref="values"/>, that defines when two values are equal.
        /// </summary>
        protected IEqualityComparer<T> comparer;

        /// <summary>
        /// Constructs a validator that determines if a given value belongs to a certain set.
        /// </summary>
        /// <param name="values">The set a value must belong to.</param>
        /// <param name="validator">A validator of the same type that can be combined with this one.</param>
        /// <param name="comparer">An optional comparer to be used when searching, that defines when two values are equal.</param>
        /// <exception cref="ArgumentNullException">Thrown if values is null.</exception>
        public OneOfValidator(IEnumerable<T> values, IValidator<T> validator = null, IEqualityComparer<T> comparer = null)
            : base(validator)
        {
            this.values = values ?? throw new ArgumentNullException(nameof(values));
            this.comparer = comparer;
        }

        /// <summary>
        /// Validates that <paramref name="value"/> belongs to a certain set.
        /// <para>A possible nested validator is executed first if one was supplied in the constructor.</para>
        /// </summary>
        /// <param name="value">The value to be searched for.</param>
        /// <returns>True if valid, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        public override bool IsValid(T value)
        {
            if (!base.IsValid(value))
                return false;
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return values.Contains(value, comparer);
        }
    }
}
