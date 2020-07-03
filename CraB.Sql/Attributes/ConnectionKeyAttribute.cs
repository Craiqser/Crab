using System;
using System.Reflection;

namespace CraB.Sql
{
	/// <summary>Определяет ключ строки подключения.</summary>
	/// <seealso cref="Attribute" />
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class ConnectionKeyAttribute : Attribute
	{
		/// <summary>Получает ключ строки подключения.</summary>
		/// <value>Ключ строки подключения.</value>
		public string ConnectionKey { get; }

		/// <summary>Инициализирует новый экземпляр класса <see cref="ConnectionKeyAttribute" />.</summary>
		/// <param name="connectionKey">Ключ строки подключения.</param>
		public ConnectionKeyAttribute(string connectionKey)
		{
			ConnectionKey = connectionKey;
		}

		/// <summary>Инициализирует новый экземпляр класса <see cref="ConnectionKeyAttribute" />, определяя ключ строки подключения из указанного типа.</summary>
		/// <param name="sourceType">Тип объекта для определения ключа строки подключения.</param>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ArgumentOutOfRangeException" />
		public ConnectionKeyAttribute(Type connectionKeySourceType)
		{
			connectionKeySourceType.NotNull(nameof(connectionKeySourceType));

			ConnectionKeyAttribute attr = connectionKeySourceType.GetCustomAttribute<ConnectionKeyAttribute>(true) ??
				throw new ArgumentOutOfRangeException(nameof(connectionKeySourceType), $"Атрибут '{GetType().Name}' задан через тип {connectionKeySourceType.Name}, который его не имеет.");

			ConnectionKey = attr?.ConnectionKey;
		}
	}
}
