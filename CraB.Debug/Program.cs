using CraB.Core;
using CraB.Web;
using System;
using System.Globalization;

namespace CraB.Debug
{
	class Program
	{
		static void Main()
		{
			Project.AssemblyAdd(typeof(IAuthUser).Assembly);

			string langId = "ru";

			Console.WriteLine($"CurrentCulture is {CultureHelper.LangId(langId)}.");

			ILocalizationService localizationService = new LocalizationService();
			localizationService.RegisterLocalizationAttribute();

			//Console.WriteLine();
			_ = Console.ReadKey();
		}
	}
}
