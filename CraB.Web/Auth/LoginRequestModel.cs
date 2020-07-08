using System.ComponentModel.DataAnnotations;

namespace CraB.Web
{
	public class LoginRequestModel
	{
		[Required(ErrorMessage = "Поле 'Login' должно быть заполнено.")]
		[Display(Name = "Login"), MinLength(3), MaxLength(20)]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Поле 'Password' должно быть заполнено.")]
		[Display(Name = "Password"), DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
