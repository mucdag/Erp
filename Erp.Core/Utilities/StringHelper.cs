using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Utilities
{
	public static class StringHelper
	{
		public static bool ValidatePhoneNumber(this string phone)
		{
			var cleaned = phone.RemoveNonNumeric();
			return cleaned.Length == 10 || cleaned.Length == 11;
		}


		public static string RemoveNonNumeric(this string phone)
		{
			return Regex.Replace(phone, @"[^0-9]+", "");
		}

		public static string Space => string.Empty.PadLeft(1);

		public static int ToInt(this object s)
		{
			var statusTryParse = int.TryParse(s.ToString(), out int result);
			return statusTryParse ? result : throw new Exception("Convert Exception");
		}

		public static string PluralizationService(string name)
		{
			var pluralizationService = new PluralizationServiceInstance();
			return pluralizationService.Singularize(name);
		}

		public static string FirstLetterToUpper(this string s)
		{
			if (s == null)
				return null;

			if (s.Length > 1)
				return char.ToUpper(s[0]) + s.Substring(1);

			return s.ToUpper();
		}

		public static string OnlyFirstLetterToUpperOtherToLower(this string s)
		{
			if (s == null)
				return null;

			if (s.Length > 1)
				return char.ToUpper(s[0]) + s.Substring(1).ToLower();

			return s.ToUpper();
		}

		public static string EveryWordOnlyFirstLetterToUpperOtherToLower(this string s)
		{
			if (s == null)
				return null;

			if (s.Length > 1)
			{
				var word = string.Empty;
				var words = s.Trim().Split(' ');
				foreach (var item in words)
				{
					word += item.OnlyFirstLetterToUpperOtherToLower() + " ";
				}
				return word.Trim();
			}

			return s.ToUpper();
		}

		public static string Shuffle(this string s)
		{
			var rnd = new Random();
			var result = s.OrderBy(item => rnd.Next()).ToArray();
			return new string(result);
		}

        public static string ToLikeParameter(this string s)
        {
            return $"%{s}%";
        }

    }
}