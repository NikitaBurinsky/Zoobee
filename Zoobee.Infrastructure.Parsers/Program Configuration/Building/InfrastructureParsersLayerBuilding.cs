using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Data;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;
using Zoobee.Infrastructure.Parsers.Services.Storage;

namespace Zoobee.Infrastructure.Parsers.Program_Configuration.Building
{
	public static class InfrastructureParsersLayerBuilding
	{
		public static IServiceCollection AddInfrastructureParsers(this IServiceCollection services, IConfiguration configuration, bool UseInMemoryDataBase = false)
		{
			AddServices(services);
			AddRepositories(services);
			AddParsersDbContext(services, configuration, UseInMemoryDataBase);
			return services;
		}

		private static IServiceCollection AddServices(IServiceCollection services)
		{


			return services;
		}


		private static IServiceCollection AddRepositories(IServiceCollection services)
		{
			services.AddScoped<IRawPageRepository, RawPageRepository>();
			return services;
		}

		private static IServiceCollection AddParsersDbContext(IServiceCollection services, IConfiguration configuration, bool UseInMemoryDatabase)
		{
			services.AddDbContext<IParsersDbContext, ParsersDbContext>(o =>
			{
				if (UseInMemoryDatabase)
				{
					o.UseInMemoryDatabase("ParsersInMemoryDatabase");
				}
				else
				{
					o.UseNpgsql(configuration.GetConnectionString("ParsersDatabaseConnection"),
						b => b.MigrationsAssembly("Zoobee.Infrastructure.Parsers"));
				}
			});
			return services;
		}


	}
}
