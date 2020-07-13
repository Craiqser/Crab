using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace CraB.Core
{
	/// <summary>Класс доступа к настройкам с маппингом классов.</summary>
	public static class Config
	{
		private static IConfiguration _configuration;

		private static IConfiguration Configuration
		{
			get
			{
				_configuration ??= Dependencies.Resolve<IConfiguration>();
				return _configuration;
			}
		}

		/// <summary>Загружает конфигурацию в класс указанного типа.</summary>
		/// <param name="type">Тип класса (необязательный) или тип для чтения атрибута <see cref="SettingKeyAttribute"/>.</param>
		/// <returns>Созданный объект класса данного типа с загруженными настройками.</returns>
		public static T Get<T>(Type type = null) where T : class, new()
		{
			Type typeT = type ?? typeof(T);
			SettingKeyAttribute keyAttribute = typeT.GetCustomAttribute<SettingKeyAttribute>();
			string key = (keyAttribute == null) ? typeT.Name : keyAttribute.SettingKey;

			T instance = new T();
			Configuration.Bind(key, instance);

			return instance;
		}
	}
}
