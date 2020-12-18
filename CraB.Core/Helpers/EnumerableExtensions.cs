using System;
using System.Collections;
using System.Collections.Generic;

namespace CraB.Core
{
	/// <summary>Методы расширения для работы с коллекциями.</summary>
	public static class EnumerableExtensions
	{
		/// <summary>Добавляет несколько элементов в список.</summary>
		/// <typeparam name="T">Тип элементов.</typeparam>
		/// <param name="list">Список.</param>
		/// <param name="values">Значения.</param>
		public static void AddRange<T>(this ICollection<T> list, params T[] values)
		{
			foreach (T value in values)
			{
				list.Add(value);
			}
		}

		/// <summary>Добавляет несколько элементов в список.</summary>
		/// <typeparam name="T">Тип элементов.</typeparam>
		/// <param name="list">Список.</param>
		/// <param name="values">Значения.</param>
		public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> values)
		{
			foreach (T value in values)
			{
				list.Add(value);
			}
		}

		/// <summary>Выполнение действия над каждым элементом списка.</summary>
		/// <typeparam name="T">Тип перечисляемых элементов.</typeparam>
		/// <param name="itemsEnumerable">Элементы.</param>
		/// <param name="itemAction">Действие над элементом.</param>
		public static void ForEach<T>(this IEnumerable<T> itemsEnumerable, Action<T> itemAction)
		{
			foreach (T item in itemsEnumerable)
			{
				itemAction(item);
			}
		}

		/// <summary>Получает значение указанного ключа или возвращает значение по умолчанию для TValue, если ключ не найден.</summary>
		/// <typeparam name="TKey">Тип ключа.</typeparam>
		/// <typeparam name="TValue">Тип значения.</typeparam>
		/// <param name="dictionary">Словарь.</param>
		/// <param name="key">Ключ.</param>
		/// <returns>Значение указанного ключа или значение по умолчанию для TValue, если ключ не найден.</returns>
		public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			return !dictionary.TryGetValue(key, out TValue value) ? default : value;
		}

		/// <summary>Получает значение указанного ключа или возвращает переданное значение по умолчанию, если ключ не найден.</summary>
		/// <typeparam name="TKey">Тип ключа.</typeparam>
		/// <typeparam name="TValue">Тип значения.</typeparam>
		/// <param name="dictionary">Словарь.</param>
		/// <param name="key">Ключ.</param>
		/// <param name="defaultValue">Значение по-умолчанию.</param>
		/// <returns>Значение указанного ключа или переданное значение по умолчанию, если ключ не найден.</returns>
		public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			return !dictionary.TryGetValue(key, out TValue value) ? defaultValue : value;
		}

		/// <summary>Является ли коллекция пустой или равна <c>null</c>.</summary>
		/// <param name="collection">Коллекция.</param>
		/// <returns><c>True</c>, если коллекция пустая или равна <c>null</c>, иначе <c>false</c>.</returns>
		public static bool NullOrEmpty(this ICollection collection)
		{
			return (collection == null) || (collection.Count == 0);
		}

		/// <summary>Добавляет к словарю диапазон значений.</summary>
		/// <typeparam name="TKey">Ключ.</typeparam>
		/// <typeparam name="TValue">Значение.</typeparam>
		/// <param name="dictionary">Текущий словарь.</param>
		/// <param name="range">Добавляемый диапазон значений.</param>
		public static void RangeAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> range)
		{
			range.ForEach(item => dictionary.Add(item.Key, item.Value));
		}

		/// <summary>Добавляет к словарю диапазон значений, пропуская уже существующие элементы.</summary>
		/// <typeparam name="TKey">Ключ.</typeparam>
		/// <typeparam name="TValue">Значение.</typeparam>
		/// <param name="dictionary">Текущий словарь.</param>
		/// <param name="range">Добавляемый диапазон значений.</param>
		public static void RangeAddNew<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> range)
		{
			range.ForEach(item =>
			{
				if (!dictionary.ContainsKey(item.Key))
				{
					dictionary.Add(item.Key, item.Value);
				}
			});
		}

		/// <summary>Добавляет к словарю диапазон значений с перезаписью уже существующих элементов.</summary>
		/// <typeparam name="TKey">Ключ.</typeparam>
		/// <typeparam name="TValue">Значение.</typeparam>
		/// <param name="dictionary">Текущий словарь.</param>
		/// <param name="range">Добавляемый диапазон значений.</param>
		public static void RangeAddOverride<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> range)
		{
			range.ForEach(item => dictionary[item.Key] = item.Value);
		}
	}
}
