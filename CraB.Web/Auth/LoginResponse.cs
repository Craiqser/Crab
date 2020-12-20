namespace CraB.Web.Auth
{
	/// <summary>Ответ сервера на попытку входа пользователя.</summary>
	public class LoginResponse
	{
		/// <summary>Ключ ошибки.</summary>
		public string ErrorKey { get; init; }

		public bool Successful => UserPayload is not null;

		/// <summary>Данные пользователя.</summary>
		public IUserPayload UserPayload { get; init; }
	}
}
