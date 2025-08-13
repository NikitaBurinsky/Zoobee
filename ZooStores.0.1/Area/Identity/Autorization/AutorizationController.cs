using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZooStorages.Domain.DataEntities.Identity;
using ZooStores.Web.Area.Identity.Accounts.Models;
using ZooStores.Web.Area.Identity.Registration.Models;

namespace ZooStores.Web.Area.Identity.Registration
{
	[ApiController]
	[Route("accounts")]
	public class AutorizationController : ControllerBase
	{
		private readonly UserManager<TestUser> _userManager;
		private readonly SignInManager<TestUser> _signInManager;

		[HttpPost("register")]
		public async Task<IActionResult> Registration(
			RegistrationRequestModel registrationRequest)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var newUser = new TestUser
			{
				Email = registrationRequest.Email,
				UserName = registrationRequest.Email,
				Year = registrationRequest.BornYear,
			};
			var res = await _userManager.CreateAsync(newUser, registrationRequest.Password);
			if (res.Succeeded)
			{
				await _signInManager.SignInAsync(newUser, false);
				return Ok("Registration Successed");
			}
			else
			{
				foreach (var error in res.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return BadRequest(ModelState);
			}
		}

		[HttpPost("login")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginRequestModel requestModel)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var res = await _signInManager.PasswordSignInAsync(requestModel.Email, requestModel.Password, requestModel.RememberMe, false);
			if (res.Succeeded) {
				return Ok("Login Succesed");
			}
			else {
				ModelState.AddModelError("Login", "Incorrect password");
			}
			return BadRequest(ModelState);
		}

		[HttpPost("logout")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Ok();
		}

		public AutorizationController(UserManager<TestUser> userManager, SignInManager<TestUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
	}
}
