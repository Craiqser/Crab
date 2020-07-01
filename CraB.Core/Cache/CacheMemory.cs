using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CraB.Core.Cache
{
	/// <summary>Реализация кэширования в памяти.</summary>
	/// <typeparam name="T">Тип кэшируемых значений.</typeparam>
	/// <remarks><para><example>
	/// Создаём кэш:<code>var recordsCache = new CacheMemory&lt;record>();</code>
	/// Пример использования (синхронный, потокобезопасный):<code>var record = recordsCache.Value(id, () => db.RecordGet(id));</code>
	/// Пример использования (асинхронный, потокобезопасный, с ожиданием инициализируемого элемента):<code>var record = await recordsCache.Value(id, async () => await db.RecordGet(id));</code>
	/// </example></para></remarks>
	public class CacheMemory<T> : ICache<T>, IDisposable
	{
		private readonly MemoryCache _cache = null;
		private bool _disposed = false;
		private readonly ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();
		private readonly MemoryCacheEntryOptions _memoryCacheEntryOptionsDefault = null;

		private static readonly MemoryCacheEntryOptions _memoryCacheEntryOptionsPreset = new MemoryCacheEntryOptions
		{
			Size = 1, // Размер каждого кэшированного элемента считаем за единицу.
			Priority = CacheItemPriority.Normal, // Приоритет (Low, Normal, High, NeverRemove).
			SlidingExpiration = TimeSpan.FromMinutes(5), // Храним по-умолчанию 5 минут, если был запрос значения, то продляем хранение ещё на пять минут и т.д.
			AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) // По-умолчанию храним час. Можно переопределить в конструкторе кэша. Удаляется независимо от SlidingExpiration.
		};

		/// <summary>Создает новый экземпляр <see cref="CacheMemory&lt;T&rt;"/> с указанными параметрами кэша и параметрами его элементов.</summary>
		/// <param name="memoryCacheOptions">Параметры кэша.</param>
		/// <param name="memoryCacheEntryOptions">Параметры кэширования элемента.</param>
		public CacheMemory(MemoryCacheOptions memoryCacheOptions, MemoryCacheEntryOptions memoryCacheEntryOptions)
		{
			_cache = new MemoryCache(memoryCacheOptions);
			_memoryCacheEntryOptionsDefault = memoryCacheEntryOptions;
		}

		/// <summary>Создает новый экземпляр <see cref="CacheMemory&lt;T&rt;"/> с указанными параметрами.</summary>
		/// <param name="memoryCacheOptions">Параметры кэша.</param>
		public CacheMemory(MemoryCacheOptions memoryCacheOptions) : this(memoryCacheOptions, _memoryCacheEntryOptionsPreset)
		{ }

		/// <summary>Создает новый экземпляр <see cref="CacheMemory&lt;T&rt;"/> с лимитом количества элементов кэша.</summary>
		/// <param name="sizeLimit">Лимит количества элементов кэша.</param>
		public CacheMemory(long sizeLimit) : this(new MemoryCacheOptions { SizeLimit = sizeLimit })
		{ }

		/// <summary>Создает новый экземпляр <see cref="CacheMemory&lt;T&rt;"/> с параметрами по умолчанию.</summary>
		public CacheMemory() : this(new MemoryCacheOptions())
		{ }

		/// <summary>Получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="valueNew">Функция создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		/// <param name="memoryCacheEntryOptions">Параметры кэширования для создаваемого значения.</param>
		/// <returns>Кэшируемое значение указанного ключа.</returns>
		public T Value(object key, Func<T> valueNew, MemoryCacheEntryOptions memoryCacheEntryOptions = null)
		{
			if (!_cache.TryGetValue(key, out T value)) // Если не нашли, то создаём.
			{
				valueNew.NotNull(nameof(valueNew));

				value = valueNew(); // Получаем новый элемент и применяем настройки.
				MemoryCacheEntryOptions valueOptions = memoryCacheEntryOptions ?? _memoryCacheEntryOptionsDefault;
				_ = _cache.Set(key, value, valueOptions);
			}

			return value;
		}

		/// <summary>Получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="valueNew">Функция создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		/// <returns>Кэшируемое значение указанного ключа.</returns>
		public T Value(object key, Func<T> valueNew)
		{
			return Value(key, valueNew, null);
		}

		/// <summary>Асинхронно получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="valueNew">Функция асинхронного создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		/// <param name="memoryCacheEntryOptions">Параметры кэширования для создаваемого значения.</param>
		/// <returns>Кэшируемое значение указанного ключа.</returns>
		public async Task<T> Value(object key, Func<Task<T>> valueNew, MemoryCacheEntryOptions memoryCacheEntryOptions = null)
		{
			if (!_cache.TryGetValue(key, out T value)) // Если не нашли, то создаём (проверяем, не создаётся ли в другом потоке).
			{
				SemaphoreSlim locked = _locks.GetOrAdd(key, semaphore => new SemaphoreSlim(1, 1));
				await locked.WaitAsync().ConfigureAwait(false);

				try
				{
					if (!_cache.TryGetValue(key, out value))
					{
						valueNew.NotNull(nameof(valueNew));

						value = await valueNew().ConfigureAwait(false); // Получаем новый элемент и применяем настройки.
						MemoryCacheEntryOptions valueOptions = memoryCacheEntryOptions ?? _memoryCacheEntryOptionsDefault;
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

		/// <summary>Асинхронно получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="valueNew">Функция асинхронного создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		/// <returns>Кэшируемое значение указанного ключа.</returns>
		public async Task<T> Value(object key, Func<Task<T>> valueNew)
		{
			return await Value(key, valueNew, null).ConfigureAwait(false);
		}

		protected virtual void Dispose(bool disposing) // Реализация шаблона Dispose().
		{
			if (_disposed)
			{
				return;
			}

			if (disposing)
			{
				_cache.Dispose();
			}

			_disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~CacheMemory() => Dispose(false);
	}
}
