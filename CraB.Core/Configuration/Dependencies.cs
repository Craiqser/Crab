using Microsoft.Extensions.DependencyInjection;
using System;

namespace CraB.Core
{
	/// <summary>Локатор сервисов. Требует настройки контейнера через метод ServiceProviderSet.</summary>
	public static class Dependencies
	{
		/// <summary>Возвращает текущую регистрацию <see cref="IServiceProvider">.</summary>
		private static IServiceProvider _serviceProvider;

		/// <summary>Ищет через зарегистрированный провайдер сервис с типом TService.</summary>
		/// <typeparam name="T">Тип сервиса.</typeparam>
		/// <exception cref="FieldAccessException">Не найден локатор, который должен быть установлен с помощью SetResolver.</exception>
		/// <returns>Найденный сервис типа TService.</returns>
		public static T Resolve<T>() where T : class
		{
			return _serviceProvider.GetService<T>();
		}

		/// <summary>Устанавливает переданный локатор зависимостей в качестве текущего и возвращает предыдущий, если был установлен.</summary>
		/// <param name="dependencyResolver">Устанавливаемый локатор зависимостей.</param>
		public static void ServiceProviderSet(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}
	}
}
