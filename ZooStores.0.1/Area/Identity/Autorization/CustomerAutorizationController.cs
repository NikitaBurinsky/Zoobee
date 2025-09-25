using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Zoobee.Domain.DataEntities.Identity;
using Zoobee.Domain.DataEntities.Identity.Users;
using ZooStores.Web.Area.Identity.Autorization.Models;

namespace ZooStores.Web.Area.Identity.Autorization
{
    [ApiController]
    [Route("accounts")]
    public class CustomerAutorizationController : ControllerBase
    {
        private readonly UserManager<BaseApplicationUser> _userManager;
        private readonly SignInManager<BaseApplicationUser> _signInManager;
        /// <summary>
        /// По дефолту новому пользователю присваивается роль Customer
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Registration(
            RegistrationRequestModel registrationRequest)
        {
            var newUser = new CustomerUser
            {
                Email = registrationRequest.Email,
                UserName = registrationRequest.Email,
                BornYear = registrationRequest.BornYear,
            };
            var res = await _userManager.CreateAsync(newUser, registrationRequest.Password);
            if (res.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "customer");
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
        public async Task<IActionResult> Login(LoginRequestModel requestModel)
        {
            var res = await _signInManager.PasswordSignInAsync(requestModel.Email, requestModel.Password, requestModel.RememberMe, false);
            if (res.Succeeded)
            {
                return Ok("Login Succesed");
            }
            else
            {
                ModelState.AddModelError("Login", "Incorrect password");
            }
            return BadRequest(ModelState);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
        public CustomerAutorizationController(UserManager<BaseApplicationUser> userManager, SignInManager<BaseApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
    }
}
