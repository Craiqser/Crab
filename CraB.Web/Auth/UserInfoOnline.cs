using System;
using System.Collections.Generic;

namespace CraB.Web.Auth
{
	/// <summary>Токен аутентификации для онлайн-пользователя.</summary>
	public class UserInfoOnline
	{
		/// <summary>Окончание срока действия.</summary>
		public DateTime ExpiresAt { get; init; }

		/// <summary>Код пользователя.</summary>
		public int Id { get; init; }

		/// <summary>Язык интерфейса пользователя.</summary>
		public string LangId { get; init; }

		/// <summary>Логин пользователя.</summary>
		public string Login { get; init; }

		/// <summary>Токен доступа.</summary>
		public string Token { get; init; }

		/// <summary>Ключи доступа.</summary>
		public Dictionary<string, string> SecurityKeys { get; init; }
	}
}
