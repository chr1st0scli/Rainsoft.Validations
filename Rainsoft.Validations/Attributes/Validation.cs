using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rainsoft.Validations.Attributes
{
    public static class Validation
    {
        /// <summary>
        /// This is a design choice to compensate for the fact that there is no common inherited GetValue method for PropertyInfo and FieldInfo.
        /// Also, there is no common inherited Type property representing either PropertyType or FieldType depending on the runtime type.
        /// </summary>
        abstract class MemberInfoWrapper
        {
            private readonly MemberInfo _mi;

            public MemberInfoWrapper(MemberInfo mi) => _mi = mi;

            public string Name => _mi.Name;

            public object[] GetCustomAttributes(Type attributeType, bool inherit) => _mi.GetCustomAttributes(attributeType, inherit);

            public abstract Type MemberType { get; }

            public abstract object GetValue(object obj);
        }

        class PropertyInfoWrapper : MemberInfoWrapper
        {
            private readonly PropertyInfo _pi;

            public PropertyInfoWrapper(PropertyInfo pi) : base(pi) =>  _pi = pi;

            public override Type MemberType => _pi.PropertyType;

            public override object GetValue(object obj) => _pi.GetValue(obj);
        }

        class FieldInfoWrapper : MemberInfoWrapper
        {
            private readonly FieldInfo _fi;

            public FieldInfoWrapper(FieldInfo fi) : base(fi) => _fi = fi;

            public override Type MemberType => _fi.FieldType;

            public override object GetValue(object obj) => _fi.GetValue(obj);
        }

        /// <summary>
        /// Extension method to validate a class instance based on attribute validation declarations on the class itself.
        /// </summary>
        /// <typeparam name="T">The type of object to validate.</typeparam>
        /// <param name="obj">The object to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public static bool IsValid<T>(this T obj) where T : class
        {
            IList<ValidationOffense> offenses = null;
            return obj.IsValid(ref offenses, false);
        }

        /// <summary>
        /// Extension method to validate a class instance based on attribute validation declarations on the class itself.
        /// </summary>
        /// <typeparam name="T">The type of object to validate.</typeparam>
        /// <param name="obj">The object to validate.</param>
        /// <param name="offenses">If obj is invalid, it contains the validation offenses, otherwise it is empty. Null can be initially passed.</param>
        /// <param name="checkAll">True to gather all validation offenses. If false, the check stops at the first offense.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public static bool IsValid<T>(this T obj, ref IList<ValidationOffense> offenses, bool checkAll = true) where T : class
        {
            if (offenses == null)
                offenses = new List<ValidationOffense>();

            Type objectType = obj?.GetType();

            IEnumerable<MemberInfoWrapper> properties = (objectType?.GetProperties() ?? Array.Empty<PropertyInfo>())
                .Select(pi => new PropertyInfoWrapper(pi));

            IEnumerable<MemberInfoWrapper> fields = (objectType?.GetFields() ?? Array.Empty<FieldInfo>())
                .Select(fi => new FieldInfoWrapper(fi));

            foreach (var member in properties.Concat(fields))
            {
                object value = member.GetValue(obj);

                foreach (var attr in member.GetCustomAttributes(typeof(IObjectValueRule), false))
                {
                    if (attr is IObjectValueRule rule)
                    {
                        if (!rule.IsValid(value))
                        {
                            offenses.Add(new ValidationOffense
                            {
                                TypeName = objectType.Name,
                                Rule = rule,
                                PropertyName = member.Name,
                                OffendingValue = value
                            });
                            if (!checkAll)
                                return false;
                        }
                    }
                }

                // If the member is a reference type, call this function recursively.
                if (member.MemberType.IsClass && member.MemberType != typeof(string))
                {
                    if (!value.IsValid(ref offenses, checkAll) && !checkAll)
                        return false;
                }
            }
            return offenses.Count == 0;
        }
    }
}
