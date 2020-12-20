namespace CraB.Web.Auth
{
	/// <summary>Ответ сервера на аутентификацию пользователя.</summary>
	public interface IUserPayload
	{
		/// <summary>Действие токена доступа в минутах.</summary>
		public int ExpiresMin { get; init; }

		/// <summary>Имя пользователя.</summary>
		public string Name { get; init; }

		/// <summary>Токен доступа.</summary>
		public string TokenAccess { get; init; }

		/// <summary>Токен обновления.</summary>
		public string TokenRefresh { get; init; }
	}
}
