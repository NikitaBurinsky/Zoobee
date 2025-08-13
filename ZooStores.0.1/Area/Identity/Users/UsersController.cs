using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZooStorages.Domain.DataEntities.Identity;

namespace ZooStores.Web.Area.Identity.Users
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        UserManager<TestUser> _userManager;

        [HttpGet("list")]
        public IActionResult ListUsers()
        {
			return Ok(_userManager.Users.ToList());
        }	

        public UsersController(UserManager<TestUser> userManager)
        {
            _userManager = userManager;
        }
    }
}
