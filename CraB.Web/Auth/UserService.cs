using CraB.Core;
using CraB.Sql;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CraB.Web
{
	public class UserService<TUser> where TUser : IAuthUser
	{
		private static string AuthenticationTicket(string userName)
		{
			JwtSettings jwtSettings = Dependencies.Resolve<JwtSettings>();
			List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
			SigningCredentials signingCredentials = new SigningCredentials(jwtSettings.Key, SecurityAlgorithms.HmacSha256);

			JwtSecurityToken token = new JwtSecurityToken(
				issuer: jwtSettings.Issuer,
				audience: jwtSettings.Audience,
				claims: claims,
				expires: DateTime.Now.AddDays(jwtSettings.ExpiryDays),
				signingCredentials: signingCredentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private static TUser GetByUserName(string userName)
		{
			using IDbConnection connection = Connections.New("Default");
			string sql = "select t.* from dbo.Users as t with(nolock) where (t.UserName = @UserName);";

			return connection.QuerySingleOrDefault<TUser>(sql, new { UserName = userName });
		}

		public TUser Get(string userName)
		{
			return CacheApp.Value($"{CachePrefix.User}{userName}", TimeSpan.FromHours(10), () => { return GetByUserName(userName); });
		}

		public LoginResponseModel Login(LoginRequestModel loginRequestModel)
		{
			LoginResponseModel loginFailedModel = new LoginResponseModel { Successful = false, Error = "Login and password are invalid." };

			if ((loginRequestModel == null) || loginRequestModel.UserName.TrimmedEmpty())
			{
				return loginFailedModel;
			}

			Throttler throttler = new Throttler($"{CachePrefix.User}{loginRequestModel.UserName.ToUpperInvariant()}", TimeSpan.FromMinutes(15), 5);
			TUser user;

			if (loginRequestModel.Password.NullOrEmpty() || ((user = Get(loginRequestModel.UserName)) == null) || (user.Active != DeleteOffActive.Active))
			{
				_ = throttler.Check();
				return loginFailedModel;
			}

			if (Generator.ComputeSHA512(loginRequestModel.Password, user.PasswordSalt) == user.PasswordHash)
			{
				throttler.Reset();
				return new LoginResponseModel { Successful = true, Token = AuthenticationTicket(user.UserName) };
			}

			_ = throttler.Check();

			return loginFailedModel;
		}
	}
}
