using System.ComponentModel.DataAnnotations;

namespace CraB.Web
{
	public class RegisterRequestModel
	{
		[Required(ErrorMessage = "Поле 'Login' должно быть заполнено.")]
		[DataType(DataType.Text), MinLength(3), MaxLength(20)]
		[Display(Name = "Login")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Поле 'Email' должно быть заполнено.")]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Поле 'Password' должно быть заполнено.")]
		[StringLength(100, ErrorMessage = "Длина поля {0} должна быть от {2} до {1} символов.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
		public string ConfirmPassword { get; set; }
	}
}
