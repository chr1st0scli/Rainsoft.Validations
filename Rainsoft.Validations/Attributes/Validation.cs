using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rainsoft.Validations.Attributes
{
    public static class Validation
    {
        public static bool IsValid<T>(this T obj) where T : class
        {
            IList<ValidationOffense> offenses = null;
            return obj.IsValid(ref offenses, false);
        }

        public static bool IsValid<T>(this T obj, ref IList<ValidationOffense> offenses, bool checkAll = true) where T : class
        {
            if (offenses == null)
                offenses = new List<ValidationOffense>();

            foreach (var prop in obj?.GetType().GetProperties() ?? Array.Empty<PropertyInfo>())
            {
                object value = prop.GetValue(obj);

                foreach (var attr in prop.GetCustomAttributes(typeof(IObjectValueRule), false))
                {
                    if (attr is IObjectValueRule rule)
                    {
                        if (!rule.IsValid(value))
                        {
                            offenses.Add(new ValidationOffense
                            {
                                TypeName = obj.GetType().Name,
                                Rule = rule,
                                PropertyName = prop.Name,
                                OffendingValue = value
                            });
                            if (!checkAll)
                                return false;
                        }
                    }
                }

                // If the property is a reference type, call this function recursively.
                if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    if (!value.IsValid(ref offenses, checkAll) && !checkAll)
                        return false;
                }
            }
            return offenses.Count == 0;
        }
    }
}
