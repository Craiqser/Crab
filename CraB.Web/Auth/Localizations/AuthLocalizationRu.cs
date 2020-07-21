using CraB.Core;

namespace CraB.Web
{
	[Localization(LanguageId = "ru-RU", KeyPrefix = "User:Auth")]
	public static class AuthLocalizationRu
	{
		public const string AccountNotActivated = "Аккаунт не активирован.";
		public const string EmailInvalid = "Недействительный email-адрес.";
		public const string LoginInUse = "Указанный логин уже используется.";
		public const string LoginCharInvalid = "Логин содержит недопустимый символ (используйте латинские буквы, цифры, символы ., _, -, @).";
		public const string LoginLength = "Логин должен содержать от 3 до 20 символов.";
		public const string LoginPassInvalid = "Логин и пароль недействительны.";
		public const string PasswordConfirm = "Пароль и подтверждение пароля не совпадают.";
		public const string PasswordLength = "Пароль должен содержать не менее 6 символов.";
		public const string RegistrationError = "Ошибка регистрации пользователя.";
	}
}
