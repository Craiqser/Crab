using CraB.Core;
using CraB.Sql;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CraB.Web
{
	public static class UserService<TUser> where TUser : IAuthUser
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
			string sql = "select t.* from dbo.Users as t with(nolock) where (t.UserName = @UserName);";
			return Query.SingleOrDefault<TUser>(sql, new { UserName = userName });
		}

		private static async Task<TUser> GetByUserNameAsync(string userName)
		{
			string sql = "select t.* from dbo.Users as t with(nolock) where (t.UserName = @UserName);";
			return await Query.SingleOrDefaultAsync<TUser>(sql, new { UserName = userName });
		}

		public static TUser Get(string userName)
		{
			return CacheApp.Value($"{CachePrefix.User}{userName}", TimeSpan.FromHours(10), () => { return GetByUserName(userName); });
		}

		public static async Task<TUser> GetAsync(string userName)
		{
			return await CacheApp.Value($"{CachePrefix.User}{userName}", TimeSpan.FromHours(10), async () => { return await GetByUserNameAsync(userName); });
		}

		public static async Task<LoginResponseModel> LoginAsync(LoginRequestModel loginRequestModel)
		{
			LoginResponseModel loginFailedModel = new LoginResponseModel { Successful = false, Error = "Login and password are invalid." };

			if ((loginRequestModel == null) || loginRequestModel.UserName.TrimmedEmpty())
			{
				return loginFailedModel;
			}

			Throttler throttler = new Throttler($"{CachePrefix.User}{loginRequestModel.UserName.ToUpperInvariant()}", TimeSpan.FromMinutes(15), 5);
			TUser user;

			if (loginRequestModel.Password.NullOrEmpty() || ((user = await GetAsync(loginRequestModel.UserName)) == null) || (user.Active != DeleteOffActive.Active))
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
