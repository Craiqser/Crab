using System;

namespace CraB.Core
{
	/// <summary>Класс доступа к настройкам через текущего провайдера настроек конфигурации.</summary>
	public static class Config
	{
		private static IConfigurationRepository _configurationRepository;

		private static IConfigurationRepository Repository
		{
			get
			{
				_configurationRepository ??= Dependencies.Resolve<IConfigurationRepository>();
				return _configurationRepository;
			}
		}

		/// <summary>Возвращает настройки конфигурации для указанного типа настройки. Если настройка не найдена, возвращает новый экземпляр объекта.</summary>
		/// <param name="settingType">Тип настройки.</param>
		public static object Get(Type type)
		{
			return Repository.Load(type);
		}

		/// <summary>Возвращает настройки конфигурации для указанного типа настройки. Если настройка не найдена, возвращает новый экземпляр объекта.</summary>
		/// <typeparam name="T">Тип настройки.</typeparam>
		public static T Get<T>() where T : class, new()
		{
			return (T)Get(typeof(T));
		}
	}
}
