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

		/// <summary>Получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="valueNew">Функция создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		/// <param name="expiration">Время хранения (TimeSpan.Zero - постоянное хранение).</param>
		public static object Value(string key, TimeSpan expiration, Func<object> valueNew)
		{
			return Provider.Value(key, valueNew, expiration);
		}
	}
}
