using System;

namespace CraB.Core
{
	/// <summary>Обеспечивает проверку периодичности операций (например, не более 5 попыток входа за 30 минут).</summary>
	public class Throttler
	{
		private class CounterHits // Объект кэширования со значением счётчика.
		{
			public int Counter;
		}

		private readonly string _cacheKey; // Ключ кэширования.
		private readonly TimeSpan _duration; // Период проверки лимита до сброса значения.
		private readonly int _limit; // Лимит (количество разрешённых раз срабатывания метода Check()).

		/// <summary>Создаёт новый объект ограничителя.</summary>
		/// <param name="key">Ключ кэширования для ограничителя. Введите ограничиваемый ресурс (например, имя пользователя).</param>
		/// <param name="duration">Период проверки.</param>
		/// <param name="limit">Лимит (количество разрешённых раз).</param>
		public Throttler(string key, TimeSpan duration, int limit)
		{
			_cacheKey = $"Check:{key}:{duration.Ticks.ToInvariant()}";
			_duration = duration;
			_limit = limit;
		}

		/// <summary>Проверка превышения лимита.</summary>
		/// <returns><c>True</c>, если лимит не превышен, иначе <c>false</c>.</returns>
		public bool Check()
		{
			if (Cache.Value(_cacheKey, out CounterHits hit))
			{
				if (hit.Counter++ >= _limit)
				{
					return false;
				}
			}
			else
			{
				Cache.Value(_cacheKey, _duration, new CounterHits { Counter = 1 });
			}

			return true;
		}

		/// <summary>Сброс ограничителя (удаление из кэша).</summary>
		public void Reset()
		{
			Cache.Remove(_cacheKey);
		}
	}
}
