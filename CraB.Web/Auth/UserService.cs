using CraB.Core;
using CraB.Core.Helpers;
using CraB.Sql;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CraB.Web
{
	public class UserService<TUser> where TUser : class, IAuthUser
	{
		private static string AuthenticationTicket(string userName)
		{
			List<Claim> userClaims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };

			JwtSettings jwtSettings = Dependencies.Resolve<JwtSettings>();
			SigningCredentials signingCredentials = new SigningCredentials(jwtSettings.Key, SecurityAlgorithms.HmacSha256);
			JwtSecurityToken token = new JwtSecurityToken(
				issuer: jwtSettings.Issuer,
				audience: jwtSettings.Audience,
				claims: userClaims,
				expires: DateTime.Now.AddMinutes(jwtSettings.ExpiryMinutes),
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
			return await Query.SingleOrDefaultAsync<TUser>(sql, new { UserName = userName }).ConfigureAwait(false);
		}

		public TUser Get(string userName)
		{
			return CacheApp.Value($"{CachePrefix.User}{userName}", TimeSpan.FromHours(10), () => { return GetByUserName(userName); });
		}

		public async Task<TUser> GetAsync(string userName)
		{
			return await CacheApp.Value($"{CachePrefix.User}{userName}", TimeSpan.FromHours(10), async () =>
				{ return await GetByUserNameAsync(userName).ConfigureAwait(false); }).ConfigureAwait(false);
		}

		public async Task<T> LoginAsync<T>(LoginRequestModel loginRequestModel) where T : ILoginResponseModel, new()
		{
			T loginFailedModel = new T { Successful = false, ErrorDescr = "User:Auth:LoginPassInvalid" };

			if ((loginRequestModel == null) || loginRequestModel.UserName.TrimmedEmpty())
			{
				return loginFailedModel;
			}

			Throttler throttler = new Throttler($"{CachePrefix.User}{loginRequestModel.UserName.ToUpperInvariant()}", TimeSpan.FromMinutes(15), 5);
			TUser user = null;

			if (loginRequestModel.Password.NullOrEmpty() || ((user = await GetAsync(loginRequestModel.UserName).ConfigureAwait(false)) == null) || (user.Active != DeleteOffActive.Active))
			{
				if ((user != null) && user.Active == DeleteOffActive.Off)
				{
					loginFailedModel.ErrorDescr = "User:Auth:AccountNotActivated";
				}

				_ = throttler.Check();

				return loginFailedModel;
			}

			if (Generator.ComputeSHA512(loginRequestModel.Password, user.PasswordSalt) == user.PasswordHash)
			{
				throttler.Reset();
				return new T { Successful = true, Token = AuthenticationTicket(user.Login) };
			}

			_ = throttler.Check();

			return loginFailedModel;
		}

		/// <summary>Регистрация нового пользователя.</summary>
		/// <param name="registerRequestModel">Анкета пользователя.</param>
		public async Task<RegisterResponseModel> RegisterAsync(RegisterRequestModel registerRequestModel)
		{
			RegisterResponseModel registerFailedModel = new RegisterResponseModel { Successful = false, Error = "User:Auth:RegistrationError" };

			if (registerRequestModel == null)
			{
				return registerFailedModel;
			}

			registerRequestModel.UserName = registerRequestModel.UserName.TrimToEmpty();
			registerRequestModel.Email = registerRequestModel.Email.TrimToEmpty();
			registerRequestModel.Password = registerRequestModel.Password.TrimToEmpty();
			registerRequestModel.PasswordConfirm = registerRequestModel.PasswordConfirm.TrimToEmpty();

			if (registerRequestModel.UserName.Length < 3 || registerRequestModel.UserName.Length > 20)
			{
				registerFailedModel.Error = "User.Auth.LoginLength";
			}
			else
			{
				foreach (char c in registerRequestModel.UserName)
				{
					registerFailedModel.Error = "User.Auth.LoginCharInvalid";

					if (!c.LoginChar())
					{
						return registerFailedModel;
					}
				}
			}

			if (await GetAsync(registerRequestModel.UserName).ConfigureAwait(false) != null)
			{
				registerFailedModel.Error = "User.Auth.LoginInUse";
			}
			else if ((registerRequestModel.Email.Length > 0) && !registerRequestModel.Email.EmailValid())
			{
				registerFailedModel.Error = "User.Auth.EmailInvalid";
			}
			else if (registerRequestModel.Password.Length < 6)
			{
				registerFailedModel.Error = "User.Auth.PasswordLength";
			}
			else if (registerRequestModel.Password != registerRequestModel.PasswordConfirm)
			{
				registerFailedModel.Error = "User.Auth.PasswordConfirm";
			}
			else
			{
				return new RegisterResponseModel { Successful = true, Error = string.Empty };
			}

			return registerFailedModel;
		}
	}
}
