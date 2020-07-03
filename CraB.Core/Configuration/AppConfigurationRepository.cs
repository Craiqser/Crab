using System;
using System.Reflection;

namespace CraB.Core
{
	/// <summary>Реализация интерфейса IAppConfigurationRepository, которая считывает настройки из раздела "AppSettings" в appsettings.json.</summary>
	/// <seealso cref="IAppConfigurationRepository" />
	public class ConfigurationRepository : IAppConfigurationRepository
	{
		/// <summary>Загружает конфигурацию для указанного типа настройки. Возвращает экземпляр объекта, даже если настройка не найдена.</summary>
		/// <param name="type">Тип настройки или тип с атрибутом <see cref="SettingKeyAttribute"/>.</param>
		/// <returns>Загруженный или созданный объект настройки данного типа.</returns>
		/// <exception cref="ArgumentNullException" />
		public object Load(Type type)
		{
			type.NotNull(nameof(type));

			SettingKeyAttribute keyAttr = type.GetCustomAttribute<SettingKeyAttribute>();
			string key = (keyAttr == null) ? type.Name : keyAttr.SettingKey;

			return Cache.Value("AppSetting:" + type.FullName, TimeSpan.Zero, delegate ()
			{
				IConfigurationManager configuration = Dependencies.Resolve<IConfigurationManager>();

				return (configuration == null) ? Activator.CreateInstance(type) : configuration.AppSetting(key, type) ?? Activator.CreateInstance(type);
			});
		}
	}
}
