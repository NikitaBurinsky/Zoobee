using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Zoobee.Domain.DataEntities.Identity.Role;
using Zoobee.Domain.DataEntities.Identity.Users;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Seeding;

namespace Zoobee.Infrastructure.Parsers.ProgramConfigurators.AppPreRun
{
	public static class IdentityRolesInitializer
	{
		public static async void ScrapingUrlsSeeding(this WebApplication app)
		{
			using (var scope = app.Services.CreateScope())
			{
				var seeder = scope.ServiceProvider.GetRequiredService<IScrapingSeeder>();
					await seeder.SeedAsync();
			}
		}


	}
}
