﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rainsoft.Validations.Attributes
{
    public enum ValidationMode { Properties, Fields }

    public static class ValidationEngine
    {
        /// <summary>
        /// Extension method to validate a class instance based on attribute validation declarations.
        /// The validations are performed on the class itself and all other related classes via aggregation and inheritance relationships.
        /// </summary>
        /// <typeparam name="T">The type of object to validate.</typeparam>
        /// <param name="obj">The object to validate.</param>
        /// <param name="mode">The type of members to be validated, either properties or fields.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public static bool IsValid<T>(this T obj, ValidationMode mode = ValidationMode.Properties) where T : class
        {
            IList<ValidationOffense> offenses = null;
            return obj.IsValid(ref offenses, mode, false);
        }

        /// <summary>
        /// Extension method to validate a class instance based on attribute validation declarations.
        /// The validations are performed on the class itself and all other related classes via aggregation and inheritance relationships.
        /// Validation offenses are returned to let the client code know which validations failed.
        /// </summary>
        /// <typeparam name="T">The type of object to validate.</typeparam>
        /// <param name="obj">The object to validate.</param>
        /// <param name="offenses">If obj is invalid, it contains the validation offenses, otherwise it is empty. Null can be initially passed.</param>
        /// <param name="mode">The type of members to be validated, either properties or fields.</param>
        /// <param name="checkAll">True to gather all validation offenses. If false, the check stops at the first offense.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public static bool IsValid<T>(this T obj, ref IList<ValidationOffense> offenses, ValidationMode mode = ValidationMode.Properties, bool checkAll = true) where T : class
        {
            if (offenses == null)
                offenses = new List<ValidationOffense>();

            Type objectType = obj?.GetType();

            // Consider either properties or fields because auto-implemented properties can have backing fields and there is no
            // safe way to tell them apart from regular ones without relying on assumptions. So, if a property's type is a class
            // where validation attributes are declared, checking both properties and fields of the enclosing type would result in 
            // having the same validation run twice, once for the property and once for its backing field.
            foreach (var member in GetMembers(objectType, mode))
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
                                MemberName = member.Name,
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
                    if (!value.IsValid(ref offenses, mode, checkAll) && !checkAll)
                        return false;
                }
            }
            return offenses.Count == 0;
        }

        private static IEnumerable<MemberInfoWrapper> GetMembers(Type objectType, ValidationMode mode)
        {
            // If objectType is null or the base class "object" is reached, stop the recursion.
            if (objectType == null || objectType == typeof(object))
                return Enumerable.Empty<MemberInfoWrapper>();

            // Search only for the type's declared members because recursion is used to include
            // base type members. That way, even private members of a base class can be validated.
            const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            IEnumerable<MemberInfoWrapper> members = null;
            switch (mode)
            {
                case ValidationMode.Properties:
                    members = objectType.GetProperties(FLAGS)
                        .Select(pi => new PropertyInfoWrapper(pi));
                    break;
                case ValidationMode.Fields:
                    members = objectType.GetFields(FLAGS)
                        .Select(fi => new FieldInfoWrapper(fi));
                    break;
                default:
                    throw new NotImplementedException();
            }

            // Recursively call this method to include the members of its base class.
            return members.Concat(GetMembers(objectType.BaseType, mode));
        }

        #region Helper wrapper classes
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

            public PropertyInfoWrapper(PropertyInfo pi) : base(pi) => _pi = pi;

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
        #endregion
    }
}
