using System;

namespace CraB.Sql
{
	/// <summary>Содержит методы расширения для объектов.</summary>
	public static class ObjectExtensions
	{
		/// <summary>Проверяет значение параметра <paramref name="value"/> на <c>null</c> с выбрасыванием исключения.</summary>
		/// <param name="value">Проверяемое значение.</param>
		/// <param name="paramName">Название параметра (используйте <see cref="nameof()"/>).</param>
		/// <exception cref="ArgumentNullException(string?)" />
		internal static void NotNull(this object value, string paramName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		/// <summary>Проверяемое значение не равно пустому массиву объектов и не равно <c>null</c>.</summary>
		/// <param name="value">Проверяемое значение.</param>
		/// <param name="valueName">Наименование проверяемого значения.</param>
		internal static void NotNullOrEmpty(this object[] value, string valueName)
		{
			if (value.Length == 0)
			{
				throw new ArgumentOutOfRangeException($"'{valueName}' не может быть пустым массивом.", valueName);
			}
		}
	}
}
