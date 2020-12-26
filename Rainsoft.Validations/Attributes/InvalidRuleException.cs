using System;
using System.Runtime.Serialization;

namespace Rainsoft.Validations.Attributes
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

        public InvalidRuleException()
        {
        }

        public InvalidRuleException(string message) : base(message)
        {
        }

        public InvalidRuleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidRuleException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            if (serializationInfo != null)
            {
                RuleType = Type.GetType((string)serializationInfo.GetValue(nameof(RuleType), typeof(string)));
                TargetType = Type.GetType((string)serializationInfo.GetValue(nameof(TargetType), typeof(string)));
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(RuleType), RuleType.FullName);
            info.AddValue(nameof(TargetType), TargetType.FullName);
        }
    }
}
