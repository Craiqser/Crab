using System.ComponentModel.DataAnnotations;

namespace CraB.Web.Auth
{
	/// <summary>Модель данных для входа пользователя в систему.</summary>
	public class LoginRequest
	{
		[Required(ErrorMessage = "Auth.Login.Required")]
		[Display(Name = "Login"), MinLength(3), MaxLength(20)]
		public string Login { get; set; }

		[Required(ErrorMessage = "Auth.Password.Required")]
		[Display(Name = "Password"), DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
