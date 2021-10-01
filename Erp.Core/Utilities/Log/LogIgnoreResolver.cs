using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Utilities.Log
{
    public class LogIgnoreResolver : DefaultContractResolver
    {
        Type _AttributeToIgnore = null;
        public LogIgnoreResolver()
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
    }
}