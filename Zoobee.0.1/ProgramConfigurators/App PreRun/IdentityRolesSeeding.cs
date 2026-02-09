using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Zoobee.Application.Shared.Constants;
using Zoobee.Domain.DataEntities.Identity.Role;
using Zoobee.Domain.DataEntities.Identity.Users;

namespace Zoobee.Web.ProgramConfigurators.AppPreRun
{
	public static class IdentityRolesInitializer
	{
		public static void RolesSeeding(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			var rolesManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
			var userManager = services.GetRequiredService<UserManager<BaseApplicationUser>>();
			var config = services.GetRequiredService<IConfiguration>();

			InitializeAsync(userManager, rolesManager, config);

			var logger = services.GetRequiredService<ILogger<Program>>();
			var roles = rolesManager.Roles.Select(e => e.Name).ToList();
			logger.LogInformation("Роли установлены : {@Roles}", roles);
		}

		private static async Task InitializeAsync(UserManager<BaseApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration config)
		{
			var adminEmail = config["Admin:Email"];
			var adminPassword = config["Admin:Password"];

			if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
				throw new InvalidOperationException("Admin credentials are not configured.");

			if (await roleManager.FindByNameAsync(UserRoles.SuperAdmin) == null)
				await roleManager.CreateAsync(new ApplicationRole(UserRoles.SuperAdmin));

			if (await roleManager.FindByNameAsync(UserRoles.Customer) == null)
				await roleManager.CreateAsync(new ApplicationRole(UserRoles.Customer));

			if (await roleManager.FindByNameAsync(UserRoles.DatabaseAdmin) == null)
				await roleManager.CreateAsync(new ApplicationRole(UserRoles.DatabaseAdmin));

			if (await userManager.FindByNameAsync(adminEmail) == null)
			{
				var admin = new AdminUser
				{
					Email = adminEmail,
					UserName = adminEmail,
				};
				var res = await userManager.CreateAsync(admin, adminPassword);
				if (res.Succeeded)
					await userManager.AddToRoleAsync(admin, UserRoles.SuperAdmin);
			}
		}
	}
}
