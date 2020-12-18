using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Key = System.Tuple<string, string>;

namespace CraB.Core
{
	/// <summary>Реализация ILocalTextRegistry.</summary>
	public class LocalizationService : ILocalizationService
	{
		private static readonly LocalizationKeyComparer _keyComparer = new LocalizationKeyComparer();
		private readonly ConcurrentDictionary<Key, string> _resources = new ConcurrentDictionary<Key, string>(_keyComparer);

		private static string LanguageFallback(string languageId)
		{
			if (languageId.Length > 0)
			{
				int idx = languageId.LastIndexOf('-');

				if (idx > 0)
				{
					return languageId.SubstringSafe(0, idx);
				}
			}

			return string.Empty;
		}

		/// <summary>Добавляет новый перевод в реестр.</summary>
		/// <param name="languageId">Культура (например 'en-US', 'ru-RU').</param>
		/// <param name="key">Ключ (например, Enums.Month.June).</param>
		/// <param name="text">Значение.</param>
		public void Add(string languageId, string key, string text)
		{
			_resources[new Key(languageId, key)] = text;
		}

		/// <summary>Получает все ключи без учёта культуры.</summary>
		public HashSet<string> Keys()
		{
			HashSet<string> result = new HashSet<string>(StringComparer.Ordinal);

			foreach (Key searchKey in _resources.Keys)
			{
				_ = result.Add(searchKey.Item2);
			}

			return result;
		}

		/// <summary>Возвращает значение для указанного ключа, или сам ключ, если значение не найдено.</summary>
		/// <param name="languageId">Культура (например 'en-US', 'ru-RU').</param>
		/// <param name="key">Ключ (например, Enums.Month.June).</param>
		public string Value(string languageId, string key)
		{
			Key searchKey = new Key(languageId, key);

			if (!_resources.TryGetValue(searchKey, out string value))
			{
				while ((languageId = LanguageFallback(languageId)).Length > 0) // Поиск по культурам 'X' и 'X-Y', если изначально культура была 'X-Y-Z'.
				{
					searchKey = new Key(languageId, key); // Поиск по другим вариантам и на языке по-умолчанию.

					if (_resources.TryGetValue(searchKey, out value))
					{
						return value;
					}
				}

				searchKey = new Key(string.Empty, key); // В последнюю очередь делаем попытку прочитать значение для инвариантной культуры.

				if (_resources.TryGetValue(searchKey, out value))
				{
					return value;
				}

				return null;
			}

			return value;
		}

		/// <summary>Получает все доступные ключи и значения для указанной культуры и её вариантов.</summary>
		/// <param name="languageId">Идентификатор языка (например, en-US, ru-RU).</param>
		/// <returns>Словарь ключей и значений для указанной культуры.</returns>
		public Dictionary<string, string> Values(string languageId)
		{
			Dictionary<string, string> resources = new Dictionary<string, string>();

			while (languageId.Length > 0)
			{
				foreach (KeyValuePair<Key, string> item in _resources)
				{
					string key = item.Key.Item2;

					if ((item.Key.Item1 == languageId) && !resources.ContainsKey(key))
					{
						resources[key] = Value(languageId, key);
					}
				}

				languageId = LanguageFallback(languageId);
			}

			foreach (KeyValuePair<Key, string> item in _resources) // Напоследок пройдёмся по инвариантной культуре.
			{
				string key = item.Key.Item2;

				if ((item.Key.Item1.Length == 0) && !resources.ContainsKey(key))
				{
					resources[key] = Value(languageId, key);
				}
			}

			return resources;
		}
	}
}
