using System;

namespace CraB.Core
{
	/// <summary>Определяет ключ для загрузки данной настройки.</summary>
	/// <seealso cref="Attribute" />
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class SettingKeyAttribute : Attribute
	{
		/// <summary>Получает ключ настройки.</summary>
		/// <value>Ключ настройки.</value>
		public string SettingKey { get; }

		/// <summary>Инициализирует новый экземпляр класса <see cref="SettingKeyAttribute"/>.</summary>
		/// <param name="settingKey">Ключ настройки.</param>
		public SettingKeyAttribute(string settingKey)
		{
			SettingKey = settingKey;
		}
	}
}
