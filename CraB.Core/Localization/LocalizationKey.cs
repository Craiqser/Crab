using System.Globalization;

namespace CraB.Core
{
	/// <summary>Определяет ресурс локализации. Содержит ключ и неявные преобразования в строку и из строки.</summary>
	public class LocalizationKey
	{
		/// <summary>Пустой экземпляр класса.</summary>
		private static readonly LocalizationKey Empty = string.Empty;

		/// <summary>Ключ.</summary>
		public string Key { get; }

		/// <summary>Создаёт новый экземпляр с заданным ключём.</summary>
		/// <param name="key">Ключ.</param>
		public LocalizationKey(string key)
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

		/// <summary>Неявное преобразование в строку.</summary>
		public static implicit operator string(LocalizationKey keyValue)
		{
			return (keyValue == null) ? null : Get(keyValue.Key);
		}

		/// <summary>Неявное преобразование из строки, которое создаёт новый экземпляр класса <see cref="LocalizationKey" /> с заданным ключём.</summary>
		/// <param name="key">Ключ.</param>
		public static LocalizationKey ToLocalizationKey(string key)
		{
			return key.NullOrEmpty() ? Empty : new LocalizationKey(key);
		}

		/// <summary>Неявное преобразование из строки, которое создаёт новый экземпляр класса <see cref="LocalizationKey" /> с заданным ключём.</summary>
		/// <param name="key">Ключ.</param>
		public static implicit operator LocalizationKey(string key)
		{
			return ToLocalizationKey(key);
		}
	}
}
