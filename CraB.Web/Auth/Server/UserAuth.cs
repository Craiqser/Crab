using CraB.Core;

namespace CraB.Web.Auth.Server
{
	/// <summary>Данные пользователя для встроенной аутентификации/авторизации на сервере.</summary>
	public class UserAuth : IUser
	{
		/// <summary>Активность пользователя (-1 - удалён; 0 - выключен; 1 - активен).</summary>
		public DeleteOffActive Active { get; init; }

		/// <summary>Идентификатор пользователя.</summary>
		public int Id { get; init; }

		/// <summary>Язык интерфейса пользователя.</summary>
		public string LangId { get; init; }

		/// <summary>Уникальное имя пользователя - логин.</summary>
		public string Login { get; init; }

		/// <summary>Хэш пароля.</summary>
		public string PasswordHash { get; init; }

		/// <summary>Соль пароля.</summary>
		public string PasswordSalt { get; init; }
	}
}
