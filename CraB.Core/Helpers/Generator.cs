using System;

namespace CraB.Core
{
	public static class Generator
	{
		/// <summary>Генерирует строку заданной длины из <see cref="Guid"/>.</summary>
		/// <param name="length">Длина строки (максимум 32 символа).</param>
		public static string StringFromGuid(int length = 32)
		{
			if (length > 32)
			{
				throw new NotImplementedException($"Генерация строк длиной более 32 символов не реализована. Требуемая длина строки в символах: {length}.");
			}

			byte[] guid = Guid.NewGuid().ToByteArray();

			return BitConverter.ToString(guid).Replace("-", string.Empty, Invariant.Comparison).Substring(0, length);
		}
	}
}
