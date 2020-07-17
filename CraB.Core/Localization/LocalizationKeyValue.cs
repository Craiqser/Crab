namespace CraB.Core
{
	/// <summary>Определяет ресурс локализации. Содержит ключ и неявные преобразования в строку и из строки.</summary>
	public class LocalizationKeyValue
	{
		/// <summary>Пустой экземпляр класса.</summary>
		private static readonly LocalizationKeyValue Empty = string.Empty;

		/// <summary>Ключ.</summary>
		public string Key { get; }

		/// <summary>Значение.</summary>
		public string Value { get; }

		/// <summary>Создаёт новый экземпляр класса <see cref="LocalizationKeyValue" /> с заданными ключём и значением.</summary>
		/// <param name="key">Ключ.</param>
		public LocalizationKeyValue(string key, string value)
		{
			Key = key;
			Value = value;
		}

		/// <summary>Возвращает ресурс ключа.</summary>
		public override string ToString()
		{
			return Value.TrimToNull() ?? Key;
		}

		/// <summary>Неявное преобразование в строку.</summary>
		public static implicit operator string(LocalizationKeyValue keyValue)
		{
			return keyValue?.ToString();
		}

		/// <summary>Создаёт новый экземпляр класса <see cref="LocalizationKeyValue" /> с заданными ключём и значением.</summary>
		/// <param name="key">Ключ.</param>
		public static LocalizationKeyValue ToLocalizationKeyValue(string key, string value)
		{
			return key.NullOrEmpty() ? Empty : new LocalizationKeyValue(key, value);
		}

		/// <summary>Неявное преобразование из строки, которое создаёт новый экземпляр <see cref="LocalizationKeyValue" /> с заданным ключём.</summary>
		/// <param name="key">Ключ.</param>
		public static implicit operator LocalizationKeyValue(string key)
		{
			return ToLocalizationKeyValue(key, string.Empty);
		}
	}
}
