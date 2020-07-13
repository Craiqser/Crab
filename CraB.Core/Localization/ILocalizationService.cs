namespace CraB.Core
{
	/// <summary>Интерфейс реестра хранения переводов.</summary>
	public interface ILocalizationService
	{
		/// <summary>Добавляет новый перевод в реестр.</summary>
		/// <param name="languageId">Культура (например 'en-US', 'ru-RU').</param>
		/// <param name="key">Ключ (например, Enums.Month.June).</param>
		/// <param name="text">Значение.</param>
		void Add(string languageId, string key, string text);

		/// <summary>Возвращает значение для указанного ключа, или сам ключ, если значение не найдено.</summary>
		/// <param name="languageId">Культура (например 'en-US', 'ru-RU').</param>
		/// <param name="key">Ключ (например, Enums.Month.June).</param>
		string Value(string languageId, string key);
	}
}
