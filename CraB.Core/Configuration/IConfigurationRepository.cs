using System;

namespace CraB.Core
{
	/// <summary>Интерфейс репозитория конфигурации приложения.</summary>
	public interface IConfigurationRepository
	{
		/// <summary>Загружает настройку указанного типа. Возвращает экземпляр объекта, даже если настройка не найдена.</summary>
		/// <param name="settingType">Тип настройки.</param>
		object Load(Type settingType);
	}
}
