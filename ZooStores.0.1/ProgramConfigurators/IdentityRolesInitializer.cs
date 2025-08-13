using Microsoft.AspNetCore.Identity;
using ZooStorages.Domain.DataEntities.Identity;

namespace ZooStores.Web.ProgramConfigurators
{
	public static class IdentityRolesInitializerstatic 
	{
		public static async void RolesInitialization(this WebApplication app)
		{
			using (var scope = app.Services.CreateScope())
			{
				var rolesManager = app.Services.GetRequiredService<RoleManager<IdentityRole>>();
				var userManager = app.Services.GetRequiredService<UserManager<TestUser>>();
				await InitializeAsync(userManager, rolesManager);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Seeding Roles succeseded");
				Console.ResetColor(); 
			}
		}

		private static async Task InitializeAsync(UserManager<TestUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			string adminEmail = "nikburinsky@gmail.com";
			string adminPassword = "Password12!";
			if (await roleManager.FindByNameAsync("admin") == null)
				await roleManager.CreateAsync(new IdentityRole("admin"));
			

			if(await userManager.FindByNameAsync(adminEmail) == null)
			{
				TestUser admin = new TestUser
				{
					Email = adminEmail,
					UserName = adminEmail,
				};
				var res = await userManager.CreateAsync(admin, adminPassword);
				if(res.Succeeded)
					await userManager.AddToRoleAsync(admin, "admin");
			}

		}


	}
}
