using System.Collections.Generic;

namespace CraB.Core
{
	public static class CultureHelper
	{
		public static readonly List<string> Languages = new List<string> { "en-US", "es-ES", "ru-RU" };

		private static string LangIdFallback(string langId)
		{
			langId = langId.TrimToEmpty();

			if (langId.Length > 1)
			{
				langId = langId.Substring(0, 2);

				foreach (string l in  Languages)
				{
					if (l.Substring(0, 2) == langId)
					{
						return l;
					}
				}
			}

			return Languages[0];
		}

		/// <summary>Возвращает название культуры из списка поддерживаемых культур.</summary>
		/// <param name="langId">Требуемая культура.</param>
		public static  string LangId(string langId)
		{
			return Languages.Contains(langId) ? langId : LangIdFallback(langId);
		}
	}
}
