using System.Globalization;

namespace CraB.Core
{
	/// <summary>Определяет ресурс локализации. Содержит ключ и неявные преобразования в строку и из строки.</summary>
	public class KeyValue
	{
		/// <summary>Ключ.</summary>
		public string Key { get; }

		/// <summary>Пустой текстовый экземпляр.</summary>
		public static readonly KeyValue Empty = string.Empty;

		/// <summary>Создаёт новый экземпляр с заданным ключём.</summary>
		/// <param name="key">Ключ.</param>
		public KeyValue(string key)
		{
			Key = key;
		}

		/// <summary>Возвращает ресурс ключа, или <c>null</c>, если ресурс ключа не найден.</summary>
		private static string GetTry(string key)
		{
			return key.NullOrEmpty() ? null : Dependencies.Resolve<ILocalizationService>()?.Value(CultureInfo.CurrentUICulture.Name, key);
		}

		/// <summary>Возвращает ресурс ключа, или сам ключ, если ресурс ключа не найден.</summary>
		public static string Get(string key)
		{
			return GetTry(key) ?? key;
		}

		/// <summary>Возвращает ресурс ключа, или сам ключ, если ресурс ключа не найден.</summary>
		public override string ToString()
		{
			return Get(Key);
		}

		/// <summary>Неявное преобразование в строку, которое возвращает ресурс ключа, или сам ключ, если ресурс ключа не найден.</summary>
		public static implicit operator string(KeyValue keyValue)
		{
			return (keyValue == null) ? null : Get(keyValue.Key);
		}

		/// <summary>Создаёт новый экземпляр класса <see cref="KeyValue" /> с заданным текстовым ключём.</summary>
		/// <param name="key">Текстовый ключ.</param>
		public static KeyValue ToKeyValue(string key)
		{
			return key.NullOrEmpty() ? Empty : new KeyValue(key);
		}

		/// <summary>Неявное преобразование из строки, которое создаёт новый экземпляр класса <see cref="KeyValue" /> с заданным текстовым ключём.</summary>
		/// <param name="key">Текстовый ключ.</param>
		public static implicit operator KeyValue(string key)
		{
			return ToKeyValue(key);
		}
	}
}
