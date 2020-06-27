using System;
using System.Globalization;

namespace CraB.Core
{
	/// <summary>Константы и форматирование, связанные с инвариантной культурой.</summary>
	public static class Invariant
	{
		/// <summary>Перевод строки.</summary>
		public const string NL = "\r\n";

		/// <summary>Информация о числовом формате.</summary>
		public static readonly NumberFormatInfo NumberFormat = NumberFormatInfo.InvariantInfo;

		/// <summary>Информация о формате даты и времени.</summary>
		public static readonly DateTimeFormatInfo DateTimeFormat = DateTimeFormatInfo.InvariantInfo;

		/// <summary>Информация об инвариантной культуре.</summary>
		public static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

		/// <summary>Информация о сравнении строк.</summary>
		public static readonly StringComparison Comparison = StringComparison.InvariantCulture;

		/// <summary>Преобразует значение в строку, используя инвариантную культуру.</summary>
		/// <param name="value">Значение.</param>
		/// <returns>Конвертированная строка.</returns>
		public static string ToInvariant(this int value)
		{
			return value.ToString(NumberFormat);
		}

		/// <summary>Преобразует значение в строку, используя инвариантную культуру.</summary>
		/// <param name="value">Значение.</param>
		/// <returns>Конвертированная строка.</returns>
		public static string ToInvariant(this long value)
		{
			return value.ToString(NumberFormat);
		}

		/// <summary>Преобразует значение в строку, используя инвариантную культуру.</summary>
		/// <param name="value">Значение.</param>
		/// <returns>Конвертированная строка.</returns>
		public static string ToInvariant(this double value)
		{
			return value.ToString(NumberFormat);
		}

		/// <summary>Преобразует значение в строку, используя инвариантную культуру.</summary>
		/// <param name="value">Значение.</param>
		/// <returns>Конвертированная строка.</returns>
		public static string ToInvariant(this decimal value)
		{
			return value.ToString(NumberFormat);
		}
	}
}
