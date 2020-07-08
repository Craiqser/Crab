using CraB.Core;

namespace CraB.Web
{
	/// <summary>Интерфейс базового пользователя для встроенной аутентификации.</summary>
	public interface IAuthUser
	{
		/// <summary>Уникальное имя пользователя - логин.</summary>
		public string UserName { get; }

		/// <summary>Хэш пароля.</summary>
		public string PasswordHash { get; }

		/// <summary>Соль пароля.</summary>
		public string PasswordSalt { get; }

		/// <summary>Активность пользователя (-1 - удалён; 0 - выключен; 1 - активен).</summary>
		public DeleteOffActive Active { get; }
	}
}
