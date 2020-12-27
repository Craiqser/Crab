using System.Globalization;
using System.Text.RegularExpressions;

namespace CraB.Core
{
	/// <summary>Получает локализованное представление ключа.</summary>
	public class L
	{
		/// <summary>Возвращает постфикс ключа и приводит строку к нормализованному виду.</summary>
		/// <remarks>Например, если ключ равен "User.Auth.AppLogin", то метод вернёт "AppLogin".</remarks>
		/// <param name="key">Ключ.</param>
		private static string KeyPostfix(string key)
		{
			int idx = key.LastIndexOf('.');

			if (idx > 0)
			{
				key = key.SubstringSafe(++idx, key.Length);
			}

			return Regex.Replace(key, "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1", RegexOptions.Compiled).Trim();
		}

		/// <summary>Пустой экземпляр.</summary>
		public static readonly L Empty = new L("");

		/// <summary>Ключ.</summary>
		public string Key { get; }

		/// <summary>Создаёт новый экземпляр класса LocalText с заданным текстовым ключём.</summary>
		/// <param name="key">Текстовый ключ.</param>
		public L(string key)
		{
			Key = key;
		}

		/// <summary>Возвращает локализованное представление ключа, или null, если ресурс не найден.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="langId">Культура.</param>
		public static string GetTry(string key, string langId = null)
		{
			return key.NullOrEmpty()
				? null
				: (LocalizationRegistrator.LocalizationService?.Value(langId ?? CultureInfo.CurrentUICulture.Name, key));
		}

		/// <summary>Возвращает локализованное представление ключа, или сам ключ, если ресурс не найден.</summary>
		/// <param name="key">Ключ.</param>
		public static string Get(string key)
		{
			return GetTry(key) ?? KeyPostfix(key);
		}

		/// <summary>Возвращает локализованное представление ключа для указанной культуры, или сам ключ, если ресурс не найден.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="langId">Культура.</param>
		public static string Get(string key, string langId)
		{
			return GetTry(key, langId) ?? KeyPostfix(key);
		}

		/// <summary>Преобразование класса в строку, соответствующую ключу, либо сам ключ, если ресурс не найден.</summary>
		public override string ToString()
		{
			return Get(Key);
		}

		/// <summary>Неявное преобразование класса в строку, соответствующую ключу, либо сам ключ, если ресурс не найден.</summary>
		public static implicit operator string(L l)
		{
			return (l == null) ? null : Get(l.Key);
		}

		/// <summary>Неявное преобразование из строки, которое создаёт новый экземпляр класса L с заданным ключём.</summary>
		/// <param name="key">Ключ.</param>
		public static implicit operator L(string key)
		{
			return ToL(key);
		}

		/// <summary>Неявное преобразование из строки, которое создаёт новый экземпляр класса L с заданным ключём.</summary>
		/// <param name="key">Ключ.</param>
		public static L ToL(string key)
		{
			return key.NullOrEmpty() ? Empty : new L(key);
		}
	}
}
