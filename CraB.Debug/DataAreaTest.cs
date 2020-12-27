using CraB.Core;
using CraB.Sql;

namespace CraB.Debug
{
	[ConnectionKey("Work")]
	public class DataAreaTest
	{
		/// <summary>Код компании.</summary>
		public string Id { get; set; }

		/// <summary>Название компании.</summary>
		public string Name { get; set; }
	}


	[Localization(LanguageId = "ru-RU", KeyPrefix = "Auth")]
	public static class LangAuthRu
	{
		public const string AccountNo = "Нет аккаунта?";
		public const string Login = "Логин";
		public const string Password = "Пароль";
		public const string Register = "Регистрация";
		public const string SignIn = "Вход в систему";
		public const string SignInBtn = "Войти";
		public const string SignUp = "Регистрация";
	}

	[Localization(LanguageId = "en-US", KeyPrefix = "Auth")]
	public static class LangAuthEn
	{
		public const string AccountNo = "No account?";
		public const string Login = "Login";
		public const string Password = "Password";
		public const string Register = "Sign up";
		public const string SignInBtn = "Sign in";
	}
}
