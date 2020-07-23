using System;

namespace CraB.Core
{
	/// <summary>Устанавливает ключ enum-перечисления.</summary>
	/// <seealso cref="Attribute" />
	[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
	public class EnumKeyAttribute : Attribute
	{
		/// <summary>Получает ключ перечисления.</summary>
		/// <value>Ключ перечисления.</value>
		public string Value { get; private set; }

		/// <summary>Инициализирует новый экземпляр класса <see cref="EnumKeyAttribute" />.</summary>
		/// <param name="value">Значение.</param>
		public EnumKeyAttribute(string value)
		{
			Value = value;
		}
	}
}
