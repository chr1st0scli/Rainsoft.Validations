using System;
using System.Runtime.Serialization;

namespace Rainsoft.Validations.Attributes.Engine
{
    /// <summary>
    /// An exception that is meant to be thrown when a validation attribute is applied on an incompatible type.
    /// </summary>
    [Serializable]
    public class InvalidRuleException : Exception
    {
        /// <summary>
        /// The <see cref="IObjectValueRule"/> runtime type that is not compatible with a target.
        /// </summary>
        public Type RuleType { get; set; }

        /// <summary>
        /// The target's runtime type that the RuleType is not compatible with.
        /// </summary>
        public Type TargetType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRuleException"/> class.
        /// </summary>
        public InvalidRuleException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRuleException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public InvalidRuleException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRuleException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception.</param>
        public InvalidRuleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRuleException"/> class with serialized data.
        /// </summary>
        /// <param name="serializationInfo">Holds the serialized object data about the exception being thrown.</param>
        /// <param name="streamingContext">Contains contextual information about the source or destination.</param>
        protected InvalidRuleException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            if (serializationInfo != null)
            {
                RuleType = Type.GetType((string)serializationInfo.GetValue(nameof(RuleType), typeof(string)));
                TargetType = Type.GetType((string)serializationInfo.GetValue(nameof(TargetType), typeof(string)));
            }
        }

        /// <summary>
        /// When overridden in a derived class, sets the <paramref name="info"/> with information about the exception.
        /// </summary>
        /// <param name="info">Holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">Contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(RuleType), RuleType.FullName);
            info.AddValue(nameof(TargetType), TargetType.FullName);
        }
    }
}
