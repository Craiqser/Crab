using CraB.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CraB.Web
{
	/// <summary>Параметры валидации токенов.</summary>
	public class JwtSettings
	{
		public const string AuthScheme = "Bearer";
		public const string AuthTokenKeyName = "AuthNToken";
		public const string AuthType = "Jwt";

		public string Audience { get; }
		public int ExpiryMinutes { get; }
		public string Issuer { get; }
		public SecurityKey Key { get; }

		public static IEnumerable<Claim> ParseClaimsFromJwt(string jwtClaims)
		{
			List<Claim> claims = new List<Claim>();
			string tokenClaims = Encoding.UTF8.GetString(Base64Parse(jwtClaims.Split('.')[1]));
			Dictionary<string, object> keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(tokenClaims);
			claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

			return claims;
		}

		private static byte[] Base64Parse(string base64)
		{
			switch (base64.Length % 4)
			{
				case 2: base64 += "=="; break;
				case 3: base64 += "="; break;
			}

			return Convert.FromBase64String(base64);
		}

		public JwtSettings(string audience, int expiryMinutes, string issuer, SecurityKey key)
		{
			Audience = audience;
			ExpiryMinutes = expiryMinutes;
			Issuer = issuer;
			Key = key;
		}

		/// <summary>Загружает секцию конфигурационных настроек Jwt в класс <see cref="JwtSettings" />.</summary>
		/// <param name="configuration"></param>
		/// <remarks>"Jwt": {"Audience": "https://localhost", "Issuer": "https://localhost", "SecurityKey": "Test security key"}</remarks>
		/// <returns><see cref="JwtSettings" /></returns>
		public static JwtSettings FromConfiguration(IConfiguration configuration)
		{
			string audience = configuration["Jwt:Audience"] ?? "CraB.Auth.Audience"; // Для получателя в конфиге установить домен приложения.
			int expiryMinutes = Convert.ToInt32(configuration["Jwt:ExpiryMinutes"], Invariant.NumberFormat);
			string issuer = configuration["Jwt:Issuer"] ?? "CraB.Auth.Issuer"; // Для издателя в конфиге установить домен сервера с API аутентификации.
			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"] ?? "CraB.Auth.SecurityKey"));

			return new JwtSettings(audience, expiryMinutes == 0 ? 60 : expiryMinutes, issuer, key);
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
