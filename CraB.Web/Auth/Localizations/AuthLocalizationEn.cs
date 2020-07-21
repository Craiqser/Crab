using CraB.Core;

namespace CraB.Web
{
	[Localization(LanguageId = "en-US", KeyPrefix = "User:Auth")]
	public static class AuthLocalizationEn
	{
		public const string AccountNotActivated = "Account not activated.";
		public const string EmailInvalid = "Invalid email.";
		public const string LoginInUse = "Provided login already in use.";
		public const string LoginCharInvalid = "Login contains invalid character (use latin letters, numbers, symbols ., _, -, @).";
		public const string LoginLength = "Login must be between 3 and 20 characters.";
		public const string LoginPassInvalid = "Login and password are invalid.";
		public const string PasswordConfirm = "Password and password confirmation do not match.";
		public const string PasswordLength = "Password must contain at least 6 characters.";
		public const string RegistrationError = "User registration error.";
	}
}
