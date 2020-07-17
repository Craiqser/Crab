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

			Console.WriteLine($"CurrentCulture is {CultureInfo.CurrentCulture.Name}.");

			ILocalizationService localizationService = new LocalizationService();
			localizationService.RegisterLocalizationAttribute();

			//Console.WriteLine();
			Console.ReadKey();
		}
	}
}
