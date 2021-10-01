using Core.Utilities.Log;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Core.Utilities
{
	public static class JsonHelper
	{
		public static string ToJson(this object obj, bool ignoreNullValues = false)
		{
			return JsonConvert.SerializeObject(obj, ignoreNullValues ? new JsonSerializerSettings()
			{
				NullValueHandling = NullValueHandling.Ignore
			} : null);
		}

		public static string ToCamelCaseJson(this object obj)
		{
			return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			});
		}

		public static object JsonDeserialize(this string obj)
		{
			return JsonConvert.DeserializeObject(obj);
		}

		public static string FromDictionaryToJson(this Dictionary<string, string> dictionary)
		{
			var kvs = dictionary.Select(kvp => string.Format("\"{0}\":\"{1}\"", kvp.Key, string.Concat("", kvp.Value)));
			return string.Concat("{", string.Join("`", kvs), "}");
		}

		public static Dictionary<string, string> FromJsonToDictionary(this string json)
		{
			string[] keyValueArray = json.Replace("{", string.Empty).Replace("}", string.Empty).Replace("\"", string.Empty).Split('`');
			return keyValueArray.ToDictionary(item => item.Split(':')[0], item => item.Split(':')[1]);
		}
	}
}
