using Microsoft.AspNetCore.Identity;
using Zoobee.Domain.DataEntities.Identity.Role;
using Zoobee.Domain.DataEntities.Identity.Users;

namespace Zoobee.Web.ProgramConfigurators
{
    public static class IdentityRolesInitializerstatic 
	{
		public static async void RolesSeeding(this WebApplication app)
		{
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var rolesManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
				var userManager = services.GetRequiredService<UserManager<BaseApplicationUser>>();
				await InitializeAsync(userManager, rolesManager);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Seeding Roles succeseded");
				Console.ResetColor(); 
			}
		}

		private static async Task InitializeAsync(UserManager<BaseApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
		{
			//TODO Не хуево сделать
			string adminEmail = "nikburinsky@gmail.com";
			string adminPassword = "Password12!";
			if (await roleManager.FindByNameAsync("admin") == null)
				await roleManager.CreateAsync(new ApplicationRole("admin"));

			if (await roleManager.FindByNameAsync("customer") == null)
				await roleManager.CreateAsync(new ApplicationRole("customer"));

			if (await roleManager.FindByNameAsync("organisation") == null)
				await roleManager.CreateAsync(new ApplicationRole("organisation"));


			if (await userManager.FindByNameAsync(adminEmail) == null)
			{
				OrganisationUser admin = new OrganisationUser
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
