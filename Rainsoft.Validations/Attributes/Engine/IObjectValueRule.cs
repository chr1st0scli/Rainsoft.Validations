using Rainsoft.Validations.Core;

namespace Rainsoft.Validations.Attributes.Engine
{
    /// <summary>
    /// Interface that must be implemented by validation Attribute subclasses.
    /// </summary>
    public interface IObjectValueRule : IValidator<object>
    {
    }
}
