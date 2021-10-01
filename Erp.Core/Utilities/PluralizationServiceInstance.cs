﻿using PluralizationService;
using PluralizationService.English;
using System.Globalization;

namespace Core.Utilities
{
	public class PluralizationServiceInstance
	{
		private static readonly IPluralizationApi Api;
		private static readonly CultureInfo CultureInfo;

		static PluralizationServiceInstance()
		{
			var builder = new PluralizationApiBuilder();
			builder.AddEnglishProvider();

			Api = builder.Build();
			CultureInfo = new CultureInfo("en-US");
		}


		public string Pluralize(string name)
		{
			return Api.Pluralize(name, CultureInfo) ?? name;
		}

		public string Singularize(string name)
		{
			return Api.Singularize(name, CultureInfo) ?? name;
		}
	}
}
