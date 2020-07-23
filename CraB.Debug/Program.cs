using CraB.Core;
using CraB.Web;
using System;

namespace CraB.Debug
{
	class Program
	{
		static void Main()
		{
			Project.AssemblyAdd(typeof(IAuthUser).Assembly);

			string langId = "ru";

			Console.WriteLine($"CurrentCulture is {CultureHelper.LangId(langId)}.");

			// ILocalizationService localizationService = LocalizationRegistrator.LocalizationService;
			//Console.WriteLine();

			_ = Console.ReadKey();
		}
	}
}
