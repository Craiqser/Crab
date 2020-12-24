using CraB.Core;
using CraB.Core.Helpers;
using CraB.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CraB.Web.Auth.Server
{
	public class UserService<T> where T : class, IUser
	{
		private static Dictionary<string, UserInfoOnline> Tokens { get; } = new();

		private static UserInfoOnline AuthenticationTicket(UserAuth userAuth)
		{
			foreach (var token in Tokens.Where(t => t.Value.Id == userAuth.Id).ToList())
			{
				Tokens.Remove(token.Key);
			}

			JwtSettings jwtSettings = Dependencies.Resolve<JwtSettings>();
			string tokenOnline = TokenOnline();
			UserInfoOnline userInfoOnline = new UserInfoOnline
			{
				ExpiresAt = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryMinutes),
				Id = userAuth.Id,
				Login = userAuth.Login,
				SecurityKeys = new Dictionary<string, string>(),
				Token = tokenOnline
			};
			Tokens.Add(tokenOnline, userInfoOnline);

			return userInfoOnline;
		}

		private static T GetByLogin(string login)
		{
			string sql = "select top 1 t.* from dbo.Users as t with(nolock) where (t.[Login] = @Login);";
			return Query.SingleOrDefault<T>(sql, new { Login = login });
		}

		private static async Task<T> GetByLoginAsync(string login)
		{
			string sql = "select top 1 t.* from dbo.Users as t with(nolock) where (t.[Login] = @Login);";
			return await Query.SingleOrDefaultAsync<T>(sql, new { Login = login });
		}

		/// <summary>Генерация токена онлайн-пользователя.</summary>
		private static string TokenOnline()
		{
			byte[] randomNumber = new byte[32];
			using RandomNumberGenerator rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);

			return Convert.ToBase64String(randomNumber);
		}

		public T Get(string login, int expirationMin = 10)
		{
			return CacheApp.Value($"{CachePrefix.User}{login}", TimeSpan.FromMinutes(expirationMin), () => GetByLogin(login));
		}

		public async Task<T> GetAsync(string login, int expirationMin = 10)
		{
			return await CacheApp.Value($"{CachePrefix.User}{login}", TimeSpan.FromMinutes(expirationMin), async () => await GetByLoginAsync(login));
		}

		public async Task<UserAuth> GetUserAuthAsync(string login)
		{
			string sql = @"select t.Id, t.[Login], t.PasswordHash, t.PasswordSalt, t.Active from dbo.Users as t with(nolock) where (t.[Login] = @Login);";
			return await Query.SingleOrDefaultAsync<UserAuth>(sql, new { Login = login });
		}

		/// <summary>Обработка входа пользователя в систему.</summary>
		/// <param name="loginRequest">Содержит логин и пароль пользователя.</param>
		public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
		{
			Throttler throttler = new Throttler($"{CachePrefix.User}{loginRequest.Login.ToLowerInvariant()}", TimeSpan.FromMinutes(15), 5);

			if ((loginRequest is null)
				|| loginRequest.Login.TrimmedEmpty()
				|| !throttler.Check())
			{
				return new LoginResponse { ErrorKey = "User.Auth.LoginPassInvalid" };
			}

			UserAuth userAuth = null;

			if (loginRequest.Password.NullOrEmpty()
				|| ((userAuth = await GetUserAuthAsync(loginRequest.Login)) is null)
				|| (userAuth.Active != DeleteOffActive.Active))
			{
				return (userAuth is not null) && (userAuth.Active == DeleteOffActive.Off)
					? new LoginResponse { ErrorKey = "User.Auth.AccountNotActivated" }
					: new LoginResponse { ErrorKey = "User.Auth.LoginPassInvalid" };
			}

			if (Generator.ComputeSHA512(loginRequest.Password, userAuth.PasswordSalt) == userAuth.PasswordHash)
			{
				throttler.Reset();
				return new LoginResponse { UserInfo = AuthenticationTicket(userAuth) };
			}

			return new LoginResponse { ErrorKey = "User.Auth.LoginPassInvalid" };
		}

		/// <summary>Регистрация нового пользователя.</summary>
		/// <param name="registerRequestModel">Анкета пользователя.</param>
		public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
		{
			if (registerRequest is null)
			{
				return new RegisterResponse { Successful = false, ErrorKey = "User.Auth.RegistrationError" };
			}

			registerRequest.Login = registerRequest.Login.TrimToEmpty();
			registerRequest.Email = registerRequest.Email.TrimToEmpty();
			registerRequest.Password = registerRequest.Password.TrimToEmpty();
			registerRequest.PasswordConfirm = registerRequest.PasswordConfirm.TrimToEmpty();

			if (registerRequest.Login.Length < 3 || registerRequest.Login.Length > 20)
			{
				return new RegisterResponse { Successful = false, ErrorKey = "User.Auth.LoginLength" };
			}
			else
			{
				foreach (char c in registerRequest.Login)
				{
					if (!c.LoginChar())
					{
						return new RegisterResponse { Successful = false, ErrorKey = "User.Auth.LoginCharInvalid" };
					}
				}
			}

			if (await GetAsync(registerRequest.Login) != null)
			{
				return new RegisterResponse { Successful = false, ErrorKey = "User.Auth.LoginInUse" };
			}
			else if ((registerRequest.Email.Length > 0) && !registerRequest.Email.EmailValid())
			{
				return new RegisterResponse { Successful = false, ErrorKey = "User.Auth.EmailInvalid" };
			}
			else if (registerRequest.Password.Length < 6)
			{
				return new RegisterResponse { Successful = false, ErrorKey = "User.Auth.PasswordLength" };
			}
			else if (registerRequest.Password != registerRequest.PasswordConfirm)
			{
				return new RegisterResponse { Successful = false, ErrorKey = "User.Auth.PasswordConfirm" };
			}

			return new RegisterResponse { Successful = true };
		}
	}
}
