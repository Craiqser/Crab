using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CraB.Web
{
	/// <summary>Модель данных для регистрации пользователя в системе.</summary>
	public class RegisterRequest
	{
		[Required(ErrorMessage = "Auth.Login.Required")]
		[DataType(DataType.Text), MinLength(3), MaxLength(20)]
		[Display(Name = "Login")]
		public string Login { get; set; }

		[Required(ErrorMessage = "Auth.Email.Required")]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Auth.Password.Required")]
		[StringLength(100, ErrorMessage = "Длина поля {0} должна быть от {2} до {1} символов.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "Auth.Password.CompareNot")]
		public string PasswordConfirm { get; set; }

		[Required, DefaultValue("en-US")]
		[DataType(DataType.Text), MinLength(2), MaxLength(7)]
		public string LangId { get; set; }
	}
}
