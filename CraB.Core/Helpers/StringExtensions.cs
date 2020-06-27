﻿using System;

namespace CraB.Core
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

		/// <summary>Проверяет, действительно ли указанная строка имеет значение <c>null</c> или является пустой строкой ("").</summary>
		/// <param name="value">Строка для проверки.</param>
		/// <returns>Значение <c>true</c>, если строка равна <c>null</c> или пустой строке (""); в противном случае — <c>false</c>.</returns>
		public static bool NullOrEmpty(this string value)
		{
			return string.IsNullOrEmpty(value);
		}

		/// <summary>Преобразует строку к типу <see cref="int" />.</summary>
		/// <param name="value">Строка для проверки.</param>
		/// <returns>Значение типа <see cref="int" /> или <see cref="Nullable{T}" />, если параметр <paramref name="value"/> равен <c>null</c>, пустой ("") или недопустимой строке.</returns>
		public static int? ParseInt(this string value)
		{
			return int.TryParse(value, out int intValue) ? intValue : (int?)null;
		}

		/// <summary>Преобразует строку к типу <see cref="long" />.</summary>
		/// <param name="value">Строка для проверки.</param>
		/// <returns>Значение типа <see cref="long" /> или <see cref="Nullable{T}" />, если параметр <paramref name="value"/> равен <c>null</c>, пустой ("") или недопустимой строке.</returns>
		public static long? ParseLong(this string value)
		{
			return long.TryParse(value, out long longValue) ? longValue : (long?)null;
		}

		/// <summary>Аналог функции Substring, который не вызывает исключений на null-строке и на ошибках диапазона.</summary>
		/// <param name="value">Строка.</param>
		/// <param name="startIndex">Стартовый индекс.</param>
		/// <param name="maxLength">Максимальная длина.</param>
		/// <returns>Подстрока или пустая строка.</returns>
		public static string SubstringSafe(this string value, int startIndex, int maxLength)
		{
			if (value.NullOrEmpty())
			{
				return string.Empty;
			}

			int len = value.Length;

			return (startIndex >= len) || (maxLength <= 0)
				? string.Empty
				: ((startIndex + maxLength) > len) ? value.Substring(startIndex) : value.Substring(startIndex, maxLength);
		}

		/// <summary>Проверяет, действительно ли строка имеет значение <c>null</c>, является пустой ("") или содержащей только пробельные символы.</summary>
		/// <param name="value">Строка для проверки.</param>
		/// <returns>Если строка имеет значение <c>null</c>, пустая, или содержит только пробельные символы, то возвращается <c>true</c>, иначе <c>false</c>.</returns>
		public static bool TrimmedEmpty(this string value)
		{
			return value.TrimToNull() == null;
		}

		/// <summary>Сравнивает две строки, игнорируя пробелы слева или справа.</summary>
		/// <remarks><p><b>Предупреждение:</b> "\n" (конец строки), "\t" (табулятор) и некоторые другие символы также считаются пробельными. 
		/// Полный список можно найти в функции <see cref="string.Trim()" />.</p></remarks>
		/// <param name="value1">Первая строка.</param>
		/// <param name="value2">Вторая строка.</param>
		/// <returns><c>True</c>, если строки равны после обрезания, иначе <c>false</c>.</returns>
		public static bool TrimmedSame(this string value1, string value2)
		{
			return (((value1 == null) || (value1.Length == 0)) && ((value2 == null) || (value2.Length == 0))) || (TrimToNull(value1) == TrimToNull(value2));
		}

		/// <summary>Убирает в строке пробельные символы слева и справа.</summary>
		/// <param name="value">Строка для проверки.</param>
		/// <returns>Обработанная строка или пустая строка (""), но не <c>null</c>.</returns>
		public static string TrimToEmpty(this string value)
		{
			return value.NullOrEmpty() ? string.Empty : value.Trim();
		}

		/// <summary>Убирает в строке пробельные символы слева и справа.</summary>
		/// <param name="value">Строка для проверки.</param>
		/// <returns>Обработанная строка или <c>null</c>, если строка получилась пустой ("").</returns>
		public static string TrimToNull(this string value)
		{
			if (value.NullOrEmpty())
			{
				return null;
			}

			value = value.Trim();

			return (value.Length == 0) ? null : value;
		}
	}
}
