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

			ILocalizationService localizationService = new LocalizationService();
			localizationService.RegisterLocalizationAttribute();

			//Console.WriteLine();
			Console.ReadKey();
		}
	}
}
