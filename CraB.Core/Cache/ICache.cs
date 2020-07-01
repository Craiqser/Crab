using System;
using System.Threading.Tasks;

namespace CraB.Core.Cache
{
	/// <summary>Интерфейс доступа к кэшу.</summary>
	/// <typeparam name="T"></typeparam>
	public interface ICache<T>
	{
		/// <summary>Получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="valueNew">Функция создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		public T Value(object key, Func<T> valueNew);

		/// <summary>Асинхронно получает значение или создаёт новое.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="valueNew">Функция асинхронного создания кэшируемого значения для данного ключа, если оно ранее отсутствовало.</param>
		public Task<T> Value(object key, Func<Task<T>> valueNew);
	}
}
