using System;

namespace CraB.Sql
{
	/// <summary>Содержит методы расширения для строк.</summary>
	public static class StringExtensions
	{
		/// <summary>Проверяет, что указанная строка не равна <c>null</c> и не пустая ("") с выбрасыванием исключения.</summary>
		/// <param name="value">Строка для проверки.</param>
		/// <param name="paramName">Название параметра указанной строки (используйте <see cref="nameof()"/>).</param>
		/// <exception cref="ArgumentNullException(string?)" />
		internal static void NotNull(this string value, string paramName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		/// <summary>Проверяет, что указанная строка не равна <c>null</c> и не пустая ("") с выбрасыванием исключения.</summary>
		/// <param name="value">Строка для проверки.</param>
		/// <param name="paramName">Название параметра указанной строки (используйте <see cref="nameof()"/>).</param>
		/// <exception cref="ArgumentNullException(string?)" />
		/// <exception cref="ArgumentOutOfRangeException(string?, string?)" />
		internal static void NotNullOrEmpty(this string value, string paramName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(paramName);
			}

			if (value.Length == 0)
			{
				throw new ArgumentOutOfRangeException(paramName, $"'{paramName}' не может быть пустой строкой.");
			}
		}

		/// <summary>>Проверяет, что указанная строка не равна <c>null</c>, не пустая ("") и не содержит пробельных символов с выбрасыванием исключения.</summary>
		/// <param name="value">Строка для проверки.</param>
		/// <param name="valueName">Название параметра указанной строки (используйте <see cref="nameof()"/>).</param>
		internal static void NotNullOrWhiteSpace(this string value, string valueName)
		{
			value.NotNullOrEmpty(valueName);

			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException($"'{valueName}' не может содержать пробельные символы.", valueName);
			}
		}
	}
}
