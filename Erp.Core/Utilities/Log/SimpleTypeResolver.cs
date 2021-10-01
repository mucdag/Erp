using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Utilities.Log
{
    public class SimpleTypeResolver : DefaultContractResolver
    {
        Type _AttributeToIgnore = null;
        public SimpleTypeResolver()
        {
            _AttributeToIgnore = typeof(LogIgnoreAttribute);
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var list = type.GetProperties()
                        .Where(x => !x.GetCustomAttributes().Any(a => a.GetType() == _AttributeToIgnore))
                        .Select(p => new JsonProperty()
                        {
                            PropertyName = p.Name,
                            PropertyType = p.PropertyType,
                            Readable = true,
                            Writable = true,
                            ValueProvider = base.CreateMemberValueProvider(p)
                        }).ToList();
            return list;
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var propertyType = property.PropertyType;
            if (propertyType.IsPrimitive || propertyType.IsValueType || new Type[] {
                    typeof(Enum),
                    typeof(string),
                    typeof(decimal),
                    typeof(float),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)}.Contains(propertyType))
            {
                property.ShouldSerialize = instance => true;
            }
            else
            {
                property.ShouldSerialize = instance => false;
            }
            return property;
        }
    }
}