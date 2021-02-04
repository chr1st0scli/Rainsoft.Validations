using System;
using System.Reflection;

namespace Rainsoft.Validations.Attributes.Engine
{
    internal class FieldInfoWrapper : MemberInfoWrapper
    {
        private readonly FieldInfo _fi;

        public FieldInfoWrapper(FieldInfo fi) : base(fi) => _fi = fi;

        public override Type MemberType => _fi.FieldType;

        public override object GetValue(object obj) => _fi.GetValue(obj);
    }
}
