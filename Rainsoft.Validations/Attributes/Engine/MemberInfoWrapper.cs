using System;
using System.Reflection;

namespace Rainsoft.Validations.Attributes.Engine
{
    /// <summary>
    /// This is a design choice to compensate for the fact that there is no common inherited GetValue method for PropertyInfo and FieldInfo.
    /// Also, there is no common inherited Type property representing either PropertyType or FieldType depending on the runtime type.
    /// </summary>
    internal abstract class MemberInfoWrapper
    {
        private readonly MemberInfo _mi;

        public MemberInfoWrapper(MemberInfo mi) => _mi = mi;

        public string Name => _mi.Name;

        public object[] GetCustomAttributes(Type attributeType, bool inherit) => _mi.GetCustomAttributes(attributeType, inherit);

        public abstract Type MemberType { get; }

        public abstract object GetValue(object obj);
    }
}
