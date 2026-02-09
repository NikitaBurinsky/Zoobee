using Microsoft.AspNetCore.Mvc;

namespace Zoobee.Web.Area.Admin.Super_Admin
{
	public class SuperAdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
