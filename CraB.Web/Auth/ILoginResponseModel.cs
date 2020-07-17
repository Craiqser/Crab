namespace CraB.Web
{
	public interface ILoginResponseModel
	{
		public bool Successful { get; set; }
		public string Token { get; set; }
		public string ErrorDescr { get; set; }
	}
}
