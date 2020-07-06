using System;
using System.Security.Cryptography;

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

		/// <summary>Генерирует хэш длиной 86 символов для указанной строки, используя алгоритм <see cref="SHA512"/>.</summary>
		/// <param name="value">Хэшируемая строка.</param>
		public static string ComputeSHA512(string value, string salt = null)
		{
			value.NotNullOrEmpty(nameof(value));

			salt ??= StringFromGuid(16);
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(value + salt);

			using (SHA512 sha512 = SHA512.Create())
			{
				buffer = sha512.ComputeHash(buffer);
			}

			return Convert.ToBase64String(buffer).Substring(0, 86);
		}
	}
}
