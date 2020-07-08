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
		public const string AuthTokenKeyName = "AuthNToken";
		public const string AuthType = "Jwt";

		public string Audience { get; }
		public int ExpiryDays { get; }
		public string Issuer { get; }
		public SecurityKey Key { get; }

		public JwtSettings(string audience, int expiryDays, string issuer, SecurityKey key)
		{
			Audience = audience;
			ExpiryDays = expiryDays;
			Issuer = issuer;
			Key = key;
		}

		/// <summary>Загружает секцию конфигурационных настроек Jwt в класс <see cref="JwtSettings" />.</summary>
		/// <param name="configuration"></param>
		/// <remarks>"Jwt": {"Audience": "https://localhost", "Issuer": "https://localhost", "SecurityKey": "Test security key"}</remarks>
		/// <returns><see cref="JwtSettings" /></returns>
		public static JwtSettings FromConfiguration(IConfiguration configuration)
		{
			configuration.NotNull(nameof(configuration));

			string audience = configuration["Jwt:Audience"] ?? "CraB.Auth.Audience"; // Для получателя в конфиге установить домен приложения.
			int expDays = Convert.ToInt32(configuration["Jwt:ExpiryDays"], Invariant.NumberFormat);
			string issuer = configuration["Jwt:Issuer"] ?? "CraB.Auth.Issuer"; // Для издателя в конфиге установить домен сервера с API аутентификации.
			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"] ?? "CraB.Auth.SecurityKey"));

			return new JwtSettings(audience, expDays == 0 ? 5 : expDays, issuer, key);
		}

		public TokenValidationParameters TokenValidationParameters => new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = Issuer,
			ValidAudience = Audience,
			IssuerSigningKey = Key
		};
	}
}
