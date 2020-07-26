using CraB.Core;
using CraB.Web;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CraB.Debug
{
	class Program
	{
		static void Main()
		{
			Project.AssemblyAdd(typeof(IAuthUser).Assembly);

			// Настройка локализации для тестирования.
			ServiceCollection services = new ServiceCollection();
			services.AddSingleton<ILocalizationService, LocalizationService>();
			IServiceProvider provider = services.BuildServiceProvider();
			Dependencies.ServiceProviderSet(provider);

			Console.WriteLine(L.Get("User:Auth:AppLogin"));

			_ = Console.ReadKey();
		}
	}
}
