using System;

namespace CraB.Core
{
	/// <summary>Содержит вспомогательные функции для доступа к зарегистрированному в данный момент провайдеру ICache.</summary>
	public static class Cache
	{
		/// <summary>Используется для пропуска вызовов Dependency.Resolve в случае, если производительность очень критична.</summary>
		private static readonly ICache<object> _provider;

		/// <summary>Получает текущий провайдер локального кэша. Статический, или настроенный через средство разрешения зависимостей.</summary>
		public static ICache<object> Provider => _provider ?? Dependencies.Resolve<ICache<object>>();

		/// <summary>Удаляет объект из кэша.</summary>
		/// <param name="key">Ключ.</param>
		public static void Remove(object key)
		{
			Provider.Remove(key);
		}

		/// <summary>Получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="valueNew">Функция создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		/// <param name="expiration">Время хранения (TimeSpan.Zero - постоянное хранение).</param>
		public static object Value(string key, TimeSpan expiration, Func<object> valueNew)
		{
			return Provider.Value(key, valueNew, expiration);
		}

		/// <summary>Получает значение заданного типа и возвращает <c>true</c>, если значение присутствовало в кэше.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="value">Возвращаемое значение, если присутствует в кэше, иначе null.</param>
		/// <returns><c>True</c>, если значение найдено, иначе <c>false</c>.</returns>
		public static bool Value<TItem>(string key, out TItem value)
		{
			return Provider.Value(key, out value);
		}

		/// <summary>Добавляет значение в кэш.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="expiration">Время хранения (TimeSpan.Zero - постоянное хранение).</param>
		/// <param name="value">Объект, добавляемый в кэш.</param>
		public static void Value<TItem>(string key, TimeSpan expiration, TItem value)
		{
			_ = Value(key, expiration, () => value);
		}
	}
}
