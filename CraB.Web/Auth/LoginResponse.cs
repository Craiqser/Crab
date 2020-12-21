namespace CraB.Web.Auth
{
	/// <summary>Ответ сервера на попытку входа пользователя.</summary>
	public class LoginResponse
	{
		/// <summary>Ключ ошибки.</summary>
		public string ErrorKey { get; init; }

		/// <summary><c>True</c>, если аутентификация прошла успешно, иначе <c>false</c>.</summary>
		public bool Successful => UserInfo is not null;

		/// <summary>Данные пользователя.</summary>
		public UserInfoOnline UserInfo { get; init; }
	}
}
