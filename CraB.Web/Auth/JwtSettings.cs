using CraB.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace CraB.Web
{
	/// <summary>Параметры валидации токенов.</summary>
	public class JwtSettings
	{
		public const string AuthScheme = "Bearer";
		public const string AuthType = "Jwt";
		public const string UserTokenAccess = "uta";

		public int ExpiryMinutes { get; }
		public SecurityKey Key { get; }

		public JwtSettings(int expiryMinutes, SecurityKey key)
		{
			ExpiryMinutes = expiryMinutes;
			Key = key;
		}

		/// <summary>Загружает секцию конфигурационных настроек Jwt в класс <see cref="JwtSettings" />.</summary>
		/// <param name="configuration"></param>
		/// <remarks>"Jwt": { "ExpiryMinutes": 15, "SecurityKey": "YourVeryLongSecurityKey"}</remarks>
		/// <returns><see cref="JwtSettings" /></returns>
		public static JwtSettings FromConfiguration(IConfiguration configuration)
		{
			int expiryMinutes = Convert.ToInt32(configuration["Jwt:ExpiryMinutes"], Invariant.NumberFormat);
			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"]));

			return new JwtSettings(expiryMinutes == 0 ? 15 : expiryMinutes, key);
		}
	}
}
