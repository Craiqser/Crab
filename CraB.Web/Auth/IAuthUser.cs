using CraB.Core;

namespace CraB.Web
{
	/// <summary>Базовый интерфейс пользователя для встроенной аутентификации.</summary>
	public interface IAuthUser
	{
		/// <summary>Активность пользователя (-1 - удалён; 0 - выключен; 1 - активен).</summary>
		public DeleteOffActive Active { get; }

		/// <summary>Уникальное имя пользователя - логин.</summary>
		public string Login { get; }

		/// <summary>Хэш пароля.</summary>
		public string PasswordHash { get; }

		/// <summary>Соль пароля.</summary>
		public string PasswordSalt { get; }
	}
}
