namespace CraB.Web
{
	public class RegisterResponse
	{
		/// <summary>Ключ ошибки.</summary>
		public string ErrorKey { get; init; }

		/// <summary><c>True</c>, если регистрация прошла успешно, иначе <c>false</c>.</summary>
		public bool Successful => string.IsNullOrEmpty(ErrorKey);
	}
}
