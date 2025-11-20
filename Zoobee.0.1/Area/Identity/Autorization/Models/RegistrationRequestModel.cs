namespace ZooStores.Web.Area.Identity.Autorization.Models
{
	public class RegistrationRequestModel
	{
		public string Email { get; set; }
		public int BornYear { get; set; }
		public string Password { get; set; }
		public string PasswordConfirmation { get; set; }

	}
}
