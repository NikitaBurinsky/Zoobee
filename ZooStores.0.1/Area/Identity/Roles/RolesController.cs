using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZooStorages.Domain.DataEntities.Identity;

namespace ZooStores.Web.Area.Identity.Roles
{
	[ApiController]
	[Route("roles")]
	public class RolesController : Controller
	{
		RoleManager<IdentityRole> roleManager;
		UserManager<TestUser> userManager;

		[HttpGet("list")]
		public IActionResult ListRoles()
		{
			return Ok(roleManager.Roles.ToList());
		}
		[HttpPost("create")]
		public async Task<IActionResult> CreateRole(string name)
		{
			if (string.IsNullOrEmpty(name))
				return BadRequest();
			var result = await roleManager.CreateAsync(new IdentityRole(name));
			if(result.Succeeded)
			{
				return Ok("Succeseded");
			}
			else
			{
				foreach (var err in result.Errors)
					ModelState.AddModelError("", err.Description);
			}
			return BadRequest(ModelState);
		}

		[HttpDelete("delete")]
		public async Task<IActionResult> DeleteRole(string id)
		{
			IdentityRole role = await roleManager.FindByIdAsync(id);
			if (role == null)
				return NotFound();
			var res = await roleManager.DeleteAsync(role);
			if(!res.Succeeded)
			{
				foreach (var err in res.Errors)
					ModelState.AddModelError("", err.Description);
				return BadRequest(ModelState);
			}
			return Ok();
		}

		[HttpPost("update-roles-for-user")]
		public async Task<IActionResult> Edit(string userId, List<string> appliedRoles)
		{
			var user = await userManager.FindByIdAsync(userId);
			if(user != null)
			{
				var userRoles = await userManager.GetRolesAsync(user);
				var allRoles = roleManager.Roles.ToList();
				var addedRoles = appliedRoles.Except(userRoles).ToList();
				var removedRoles = userRoles.Except(appliedRoles);
				await userManager.AddToRolesAsync(user, addedRoles);
				await userManager.RemoveFromRolesAsync(user, removedRoles);
				return Ok();
			}
			return NotFound();
		}



		public RolesController(RoleManager<IdentityRole> roleManager, UserManager<TestUser> userManager)
		{
			this.roleManager = roleManager;
			this.userManager = userManager;
		}
	}
}
