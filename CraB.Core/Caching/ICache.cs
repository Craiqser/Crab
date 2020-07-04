using System;
using System.Threading.Tasks;

namespace CraB.Core
{
	/// <summary>Интерфейс доступа к кэшу.</summary>
	/// <typeparam name="T"></typeparam>
	public interface ICache<T>
	{
		/// <summary>Удаляет объект из кэша.</summary>
		/// <param name="key">Ключ.</param>
		public void Remove(object key);

		/// <summary>Получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="valueNew">Функция создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		public T Value(object key, Func<T> valueNew);

		/// <summary>Асинхронно получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="valueNew">Функция асинхронного создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		public Task<T> Value(object key, Func<Task<T>> valueNew);

		/// <summary>Получает значение или создаёт новое с указанным сроком хранения.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="valueNew">Функция создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		/// <param name="expiration">Время хранения (TimeSpan.Zero - постоянное хранение).</param>
		/// <returns>Значение с типом <typeparamref name="T"/>.</returns>
		public T Value(object key, Func<T> valueNew, TimeSpan expiration);

		/// <summary>Получает значение заданного типа и возвращает <c>true</c>, если значение присутствовало в кэше.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="value">Возвращаемое значение, если присутствует в кэше, иначе null.</param>
		/// <returns><c>True</c>, если значение найдено, иначе <c>false</c>.</returns>
		public bool Value<TItem>(object key, out TItem value);
	}
}
