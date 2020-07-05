using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CraB.Core
{
	/// <summary>Реализация кэширования в памяти.</summary>
	public static class Cache
	{
		private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
		private static readonly ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

		private static readonly MemoryCacheEntryOptions _memoryCacheEntryOptionsPreset = new MemoryCacheEntryOptions
		{
			Size = 1, // Размер каждого кэшированного элемента считаем за единицу.
			Priority = CacheItemPriority.Normal, // Приоритет (Low, Normal, High, NeverRemove).
			SlidingExpiration = TimeSpan.FromMinutes(5), // Храним по-умолчанию 5 минут, если был запрос значения, то продляем хранение ещё на пять минут и т.д.
			AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) // По-умолчанию храним час. Можно переопределить в конструкторе кэша. Удаляется независимо от SlidingExpiration.
		};

		/// <summary>Удаляет объект из кэша.</summary>
		/// <param name="key">Ключ.</param>
		public static void Remove(string key)
		{
			_cache.Remove(key);
		}

		/// <summary>Получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="valueNew">Функция создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		/// <param name="memoryCacheEntryOptions">Параметры кэширования для создаваемого значения.</param>
		/// <returns>Кэшируемое значение указанного ключа.</returns>
		public static TItem Value<TItem>(object key, Func<TItem> valueNew, MemoryCacheEntryOptions memoryCacheEntryOptions = null)
		{
			if (!_cache.TryGetValue(key, out TItem value)) // Если не нашли, то создаём.
			{
				valueNew.NotNull(nameof(valueNew));

				value = valueNew(); // Получаем новый элемент и применяем настройки.
				MemoryCacheEntryOptions valueOptions = memoryCacheEntryOptions ?? _memoryCacheEntryOptionsPreset;
				_ = _cache.Set(key, value, valueOptions);
			}

			return value;
		}

		/// <summary>Получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="expiration">Время хранения (TimeSpan.Zero - постоянное хранение).</param>
		/// <param name="value">Значение.</param>
		/// <returns>Кэшируемое значение указанного ключа.</returns>
		public static TItem Value<TItem>(string key, TimeSpan expiration, TItem value)
		{
			return _cache.Set(key, value, new MemoryCacheEntryOptions
			{
				Size = 1,
				Priority = (expiration == TimeSpan.Zero) ? CacheItemPriority.NeverRemove : CacheItemPriority.Normal,
				SlidingExpiration = expiration,
				AbsoluteExpirationRelativeToNow = expiration
			});
		}

		/// <summary>Получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="expiration">Время хранения (TimeSpan.Zero - постоянное хранение).</param>
		/// <param name="valueNew">Функция создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		/// <returns>Кэшируемое значение указанного ключа.</returns>
		public static TItem Value<TItem>(string key, TimeSpan expiration, Func<TItem> valueNew)
		{
			valueNew.NotNull(nameof(valueNew));

			return Value(key, expiration, valueNew());
		}

		/// <summary>Получает значение заданного типа и возвращает <c>true</c>, если значение присутствовало в кэше.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="value">Возвращаемое значение, если присутствует в кэше, иначе null.</param>
		/// <returns><c>True</c>, если значение найдено, иначе <c>false</c>.</returns>
		public static bool Value<TItem>(string key, out TItem value)
		{
			return _cache.TryGetValue(key, out value);
		}

		/// <summary>Асинхронно получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="valueNew">Функция асинхронного создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		/// <param name="memoryCacheEntryOptions">Параметры кэширования для создаваемого значения.</param>
		/// <returns>Кэшируемое значение указанного ключа.</returns>
		public static async Task<TItem> Value<TItem>(object key, Func<Task<TItem>> valueNew, MemoryCacheEntryOptions memoryCacheEntryOptions = null)
		{
			if (!_cache.TryGetValue(key, out TItem value)) // Если не нашли, то создаём (проверяем, не создаётся ли в другом потоке).
			{
				SemaphoreSlim locked = _locks.GetOrAdd(key, semaphore => new SemaphoreSlim(1, 1));
				await locked.WaitAsync().ConfigureAwait(false);

				try
				{
					if (!_cache.TryGetValue(key, out value))
					{
						valueNew.NotNull(nameof(valueNew));

						value = await valueNew().ConfigureAwait(false); // Получаем новый элемент и применяем настройки.
						MemoryCacheEntryOptions valueOptions = memoryCacheEntryOptions ?? _memoryCacheEntryOptionsPreset;
						_ = _cache.Set(key, value, valueOptions);
					}
				}
				finally
				{
					_ = locked.Release();
				}
			}

			return value;
		}
	}
}
