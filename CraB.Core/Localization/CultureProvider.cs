using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CraB.Core
{
	public class CultureProvider : IRequestCultureProvider
	{
		public List<CultureInfo> Cultures { get; }

		public CultureProvider(string[] cultures)
		{
			Cultures = cultures.Select(culture => new CultureInfo(culture)).Where(info => info != null).ToList();
		}

		public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
		{
			httpContext.NotNull(nameof(httpContext));

			string culture = httpContext.Request.Cookies["LanguagePreference"];

			if (culture.NullOrEmpty())
			{
				culture = null;
			}

			return Task.FromResult(new ProviderCultureResult(culture ?? "en-US", culture ?? "en-US"));
		}
	}
}
