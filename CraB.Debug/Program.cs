using CraB.Core;
using CraB.Sql;
using CraB.Web;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CraB.Debug
{
	class Program
	{
		static void Main()
		{
			/* Тестирование локализации.
			Project.AssemblyAdd(typeof(IAuthUser).Assembly);

			// Настройка локализации для тестирования.
			ServiceCollection services = new ServiceCollection();
			services.AddSingleton<ILocalizationService, LocalizationService>();
			IServiceProvider provider = services.BuildServiceProvider();
			Dependencies.ServiceProviderSet(provider);

			Console.WriteLine(L.Get("User:Auth:AppLogin"));
			*/

			/*
			// Тестирование запросов Dapper.
			Connects.Add("Work", "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Work;Data Source=.;MultipleActiveResultSets=true");
			List<DataAreaTest> companies;
			string sql = "select t.ID, t.[NAME] from dbo.DATAAREA as t with(nolock) where (t.ISVIRTUAL = 0);";
			companies = Query.Select<DataAreaTest>(sql).ToList();

			foreach (var c in companies)
			{
				Console.WriteLine($"{c.Id} - {c.Name}");
			}
			*/

			TimeSpan duration = TimeSpan.FromMinutes(5);
			Console.WriteLine($"{duration.TotalSeconds}");

			_ = Console.ReadKey();
		}
	}
}
