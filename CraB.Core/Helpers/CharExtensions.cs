namespace CraB.Core.Helpers
{
	public static class CharExtensions
	{
		/// <summary>Проверяет, является ли указанный символ цифрой.</summary>
		/// <remarks>0..9</remarks>
		/// <param name="c">Символ.</param>
		/// <returns><c>True</c>, если цифра, иначе <c>false</c>.</returns>
		public static bool DigitInvariant(this char c)
		{
			return (c >= '0') && (c <= '9');
		}

		/// <summary>Проверяет, является ли указанный символ буквой инварианта.</summary>
		/// <remarks>A..Z, a..z</remarks>
		/// <param name="c">Символ.</param>
		/// <returns><c>True</c>, если буква инварианта, иначе <c>false</c>.</returns>
		public static bool LetterInvariant(this char c)
		{
			return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
		}

		/// <summary>Проверяет, является ли указанный символ корректным символом логина.</summary>
		/// <remarks>0..9, A..Z, a..z, ., _, -, @</remarks>
		/// <param name="c">Символ.</param>
		/// <returns><c>True</c>, если корректный символ логина, иначе <c>false</c>.</returns>
		public static bool LoginChar(this char c)
		{
			return c.DigitInvariant() || c.LetterInvariant() || (c == '.') || (c == '_') || (c == '-') || (c == '@');
		}
	}
}
