using CraB.Core;

namespace CraB.Web
{
	[Localization(LanguageId = "ru-RU", Prefix = "User:Auth")]
	public static class AuthLocalizationRu
	{
		public const string LoginPassInvalid = "Логин и пароль недействительны.";
	}

	[Localization(LanguageId = "en-US", Prefix = "User:Auth")]
	public static class AuthLocalizationEn
	{
		public const string LoginPassInvalid = "Login and password are invalid.";
	}
}
