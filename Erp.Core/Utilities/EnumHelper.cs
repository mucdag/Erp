using System;
using System.ComponentModel;

namespace Core.Utilities
{
	public static class EnumHelper
	{
		public static string GetDescription(this Enum enumerationValue)
		{
			var type = enumerationValue.GetType();
			var memberInfo = type.GetMember(enumerationValue.ToString());
			if (memberInfo.Length > 0)
			{
				var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

				if (attrs.Length > 0)
				{
					return ((DescriptionAttribute)attrs[0]).Description;
				}
			}
			return enumerationValue.ToString();
		}
	}
}
