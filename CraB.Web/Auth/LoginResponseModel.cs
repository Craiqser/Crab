namespace CraB.Web
{
	public class LoginResponseModel
	{
		public bool Successful { get; set; }
		public string Token { get; set; }
		public string Error { get; set; }
	}
}
