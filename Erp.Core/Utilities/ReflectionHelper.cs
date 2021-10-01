using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Utilities
{
    public static class ReflectionHelper
    {
        public static object GetInstancePropertyValue(Type type, object instance, string fieldName)
        {
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                            | BindingFlags.Static;
            var propertyInfo = type.GetProperty(fieldName, bindFlags);
            return propertyInfo.GetValue(instance);
        }

        public static bool IsFundamentalType(this Type type)
        {
            if (type.IsGenericListType())
            {
                return type.GetGenericArguments()[0].IsFundamentalType();
            }

            return !type.IsClass || type == typeof(string) || type == typeof(DateTime);
            //return type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type==typeof(int?) ;
        }

        public static bool IsGenericListType(this Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(ICollection<>) ||
                                          type.GetGenericTypeDefinition() == typeof(IList<>));
        }
    }
}