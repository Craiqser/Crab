namespace CraB.Web.Auth
{
	public interface IUser
	{
		/// <summary>Уникальный идентификатор пользователя.</summary>
		public int Id { get; init; }

		/// <summary>Уникальное имя пользователя - логин.</summary>
		public string Login { get; init; }
	}
}
