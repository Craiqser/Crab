using CraB.Core;
using CraB.Core.Helpers;
using CraB.Sql;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CraB.Web.Auth.Server
{
	public class UserService<T> where T: class, IUserAuth
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

		private static T GetByUserName(string login)
		{
			string sql = "select t.* from dbo.Users as t with(nolock) where (t.Login = @Login);";
			return Query.SingleOrDefault<T>(sql, new { Login = login });
		}

		private static async Task<T> GetByUserNameAsync(string login)
		{
			string sql = "select t.* from dbo.Users as t with(nolock) where (t.Login = @Login);";
			return await Query.SingleOrDefaultAsync<T>(sql, new { Login = login });
		}

		public T Get(string userName, int expirationMin = 5)
		{
			return CacheApp.Value($"{CachePrefix.User}{userName}", TimeSpan.FromMinutes(expirationMin), () => GetByUserName(userName));
		}

		public async Task<T> GetAsync(string userName, int expirationMin = 5)
		{
			return await CacheApp.Value($"{CachePrefix.User}{userName}", TimeSpan.FromMinutes(expirationMin),
				async () => await GetByUserNameAsync(userName));
		}

		public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
		{
			if ((loginRequest is null) || loginRequest.Login.TrimmedEmpty())
			{
				return new LoginResponse { ErrorKey = "User.Auth.LoginPassInvalid" };
			}

			Throttler throttler = new Throttler($"{CachePrefix.User}{loginRequest.Login.ToUpperInvariant()}", TimeSpan.FromMinutes(15), 5);
			T user = null;

			if (loginRequest.Password.NullOrEmpty() || ((user = await GetAsync(loginRequest.Login)) is null) || (user.Active != DeleteOffActive.Active))
			{
				LoginResponse loginResponse;

				if ((user is not null) && user.Active == DeleteOffActive.Off)
				{
					loginResponse = new LoginResponse { ErrorKey = "User.Auth.AccountNotActivated" };
				}
				else
				{
					loginResponse = new LoginResponse { ErrorKey = "User.Auth.LoginPassInvalid" };
				}

				_ = throttler.Check();

				return loginResponse;
			}

			if (Generator.ComputeSHA512(loginRequest.Password, user.PasswordSalt) == user.PasswordHash)
			{
				throttler.Reset();

				LoginResponse loginResponse = new LoginResponse();
				/*
				LoginResponse loginResponse = new LoginResponse
				{
					UserPayload = new IUserPayload
					{
						ExpiresMin = 15,
						Name = user.Login,
						TokenAccess = AuthenticationTicket(user.Login),
						TokenRefresh = ""
					}
				};
				*/

				return loginResponse;
			}

			_ = throttler.Check();

			return new LoginResponse { ErrorKey = "User.Auth.LoginPassInvalid" };
		}

		/// <summary>Регистрация нового пользователя.</summary>
		/// <param name="registerRequestModel">Анкета пользователя.</param>
		public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
		{
			RegisterResponse registerFailedModel = new RegisterResponse { Successful = false, Error = "User:Auth:RegistrationError" };

			if (registerRequest is null)
			{
				return registerFailedModel;
			}

			registerRequest.Login = registerRequest.Login.TrimToEmpty();
			registerRequest.Email = registerRequest.Email.TrimToEmpty();
			registerRequest.Password = registerRequest.Password.TrimToEmpty();
			registerRequest.PasswordConfirm = registerRequest.PasswordConfirm.TrimToEmpty();

			if (registerRequest.Login.Length < 3 || registerRequest.Login.Length > 20)
			{
				registerFailedModel.Error = "User.Auth.LoginLength";
			}
			else
			{
				foreach (char c in registerRequest.Login)
				{
					registerFailedModel.Error = "User.Auth.LoginCharInvalid";

					if (!c.LoginChar())
					{
						return registerFailedModel;
					}
				}
			}

			if (await GetAsync(registerRequest.Login) != null)
			{
				registerFailedModel.Error = "User.Auth.LoginInUse";
			}
			else if ((registerRequest.Email.Length > 0) && !registerRequest.Email.EmailValid())
			{
				registerFailedModel.Error = "User.Auth.EmailInvalid";
			}
			else if (registerRequest.Password.Length < 6)
			{
				registerFailedModel.Error = "User.Auth.PasswordLength";
			}
			else if (registerRequest.Password != registerRequest.PasswordConfirm)
			{
				registerFailedModel.Error = "User.Auth.PasswordConfirm";
			}
			else
			{
				return new RegisterResponse { Successful = true, Error = string.Empty };
			}

			return registerFailedModel;
		}
	}
}
