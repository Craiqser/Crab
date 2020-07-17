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
			T loginFailedModel = new T { Successful = false, ErrorDescr = "Login and password are invalid." };

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
					loginFailedModel.ErrorDescr = "Аккаунт не активирован.";
				}

				_ = throttler.Check();

				return loginFailedModel;
			}

			if (Generator.ComputeSHA512(loginRequestModel.Password, user.PasswordSalt) == user.PasswordHash)
			{
				throttler.Reset();
				return new T { Successful = true, Token = AuthenticationTicket(user.UserName) };
			}

			_ = throttler.Check();

			return loginFailedModel;
		}

		/// <summary>Регистрация нового пользователя.</summary>
		/// <param name="registerRequestModel">Анкета пользователя.</param>
		public async Task<RegisterResponseModel> RegisterAsync(RegisterRequestModel registerRequestModel)
		{
			RegisterResponseModel registerFailedModel = new RegisterResponseModel { Successful = false, Error = "Registration model is null." };

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
				registerFailedModel.Error = "'Login' must be between 3 and 20 characters.";
			}
			else
			{
				foreach (char c in registerRequestModel.UserName)
				{
					registerFailedModel.Error = "Недопустимый символ в поле 'Login'.";

					if (!c.LoginChar())
					{
						return registerFailedModel;
					}
				}
			}

			if (await GetAsync(registerRequestModel.UserName).ConfigureAwait(false) != null)
			{
				registerFailedModel.Error = "Provided login already in use.";
			}
			else if ((registerRequestModel.Email.Length > 0) && !registerRequestModel.Email.EmailValid())
			{
				registerFailedModel.Error = "Invalid email.";
			}
			else if (registerRequestModel.Password.Length < 6)
			{
				registerFailedModel.Error = "Поле 'Password' должно быть не менее 6 символов.";
			}
			else if (registerRequestModel.Password != registerRequestModel.PasswordConfirm)
			{
				registerFailedModel.Error = "Поля 'Пароль' и 'Подтверждение пароля' не совпадают.";
			}
			else
			{
				return new RegisterResponseModel { Successful = true, Error = string.Empty };
			}

			return registerFailedModel;
		}
	}
}
