namespace ZooStores.Web.Area.Identity.Accounts.Models
{
	public class LoginRequestModel
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}
}
