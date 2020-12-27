﻿namespace Rainsoft.Validations.Core
{
    /// <summary>
    /// Iterface that defines an abstraction for validating data.
    /// </summary>
    /// <typeparam name="T">The type of data to be validated.</typeparam>
    public interface IValidator<in T>
    {
        /// <summary>
        /// Checks that a value is valid.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <returns>True if valid, false otherwise.</returns>
        bool IsValid(T value);
    }
}