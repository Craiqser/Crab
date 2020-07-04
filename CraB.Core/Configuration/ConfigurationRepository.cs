using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace CraB.Core
{
	/// <summary>Реализация IConfigurationRepository, которая считывает настройки из appsettings.json.</summary>
	/// <seealso cref="IConfigurationRepository" />
	public class ConfigurationRepository : IConfigurationRepository
	{
		/// <summary>Загружает конфигурацию для указанного типа настройки.</summary>
		/// <param name="type">Тип настройки или тип с атрибутом <see cref="SettingKeyAttribute"/>.</param>
		/// <returns>Загруженный объект настройки данного типа.</returns>
		/// <exception cref="ArgumentNullException" />
		public object Load(Type type)
		{
			type.NotNull(nameof(type));

			SettingKeyAttribute keyAttr = type.GetCustomAttribute<SettingKeyAttribute>();
			string key = (keyAttr == null) ? type.Name : keyAttr.SettingKey;

			return Cache.Value($"AppSetting:{type.FullName}", TimeSpan.Zero, delegate ()
			{
				IConfiguration configuration = Dependencies.Resolve<IConfiguration>();

				if (configuration == null)
				{
					return null;
				}

				string value = configuration[key];

				return (value is object) ? value : Activator.CreateInstance(type);
			});
		}
	}
}
