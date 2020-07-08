using CraB.Core;
using CraB.Sql;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CraB.Web
{
	/// <summary>Базовый класс пользователя, который будет расширяться в конкретном приложении.</summary>
	[ConnectionKey("Default"), TableName("Users")]
	public class AuthUser : IAuthUser
	{
		[DisplayName("Login"), MinLength(3), MaxLength(20)]
		public string UserName { get; }

		[DataType(DataType.Password), MaxLength(86)]
		public string PasswordHash { get; set; }

		[DataType(DataType.Password), MaxLength(16)]
		public string PasswordSalt { get; set; }

		[DisplayName("Active"), DefaultValue(0)]
		public DeleteOffActive Active { get; set; }
	}
}
