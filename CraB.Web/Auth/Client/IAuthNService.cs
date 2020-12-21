using System.Threading.Tasks;

namespace CraB.Web.Auth.Client
{
	public interface IAuthNService
	{
		/// <summary>Вход пользователя в систему.</summary>
		/// <param name="loginRequest"></param>
		Task<LoginResponse> Login(LoginRequest loginRequest);

		/// <summary>Выход пользователя из системы.</summary>
		Task Logout();

		/// <summary>Регистрация пользователя в системе.</summary>
		/// <param name="registerRequest"></param>
		Task<RegisterResponse> Register(RegisterRequest registerRequest);

		/// <summary>Обновление токена.</summary>
		/// <returns>Токен доступа.</returns>
		Task<string> TokenRefresh();
	}
}
