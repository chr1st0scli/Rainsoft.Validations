using System;
using System.Reflection;

namespace Rainsoft.Validations.Attributes.Engine
{
    internal class PropertyInfoWrapper : MemberInfoWrapper
    {
        private readonly PropertyInfo _pi;

        public PropertyInfoWrapper(PropertyInfo pi) : base(pi) => _pi = pi;

        public override Type MemberType => _pi.PropertyType;

        public override object GetValue(object obj) => _pi.GetValue(obj);
    }
}
