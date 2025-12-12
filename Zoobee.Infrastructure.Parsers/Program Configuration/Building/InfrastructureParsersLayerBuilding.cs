using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Core.Configuration;
using Zoobee.Infrastructure.Parsers.Core.Configurations;
using Zoobee.Infrastructure.Parsers.Data;
using Zoobee.Infrastructure.Parsers.Hosts;
using Zoobee.Infrastructure.Parsers.Interfaces.Repositories;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Downloader;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Scheduling;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Seeding;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Transformation;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;
using Zoobee.Infrastructure.Parsers.Interfaces.Transformation;
using Zoobee.Infrastructure.Parsers.Services.Downloader;
using Zoobee.Infrastructure.Parsers.Services.Scheduling;
using Zoobee.Infrastructure.Parsers.Services.Seeding;
using Zoobee.Infrastructure.Parsers.Services.Storage;
using Zoobee.Infrastructure.Parsers.Services.Transformation;
using Zoobee.Infrastructure.Parsers.Workers;

namespace Zoobee.Infrastructure.Parsers.Program_Configuration.Building
{
	public static class InfrastructureParsersLayerBuilding
	{
		public static IServiceCollection AddInfrastructureParsers(this IServiceCollection services, IConfiguration configuration, bool UseInMemoryDataBase = false)
		{
			AddServices(services);
			AddRepositories(services);
			AddParsersDbContext(services, configuration, UseInMemoryDataBase);
			AddHosts(services);
			AddConfigurations(services, configuration);
			return services;
		}

		private static IServiceCollection AddConfigurations(IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<ScrapingOptions>(
				configuration.GetSection(ScrapingOptions.SectionName));
			services.Configure<ScrapingSeedingOptions>(
				configuration.GetSection("ScrapingSeeding"));
			return services;
		}

		private static IServiceCollection AddHosts(IServiceCollection services)
		{
			services.AddHostedService<ScrapingWorker>();
			services.AddHostedService<TransformationWorker>();

			return services;
		}

		private static IServiceCollection AddServices(IServiceCollection services)
		{
			services.AddHttpClient<IHtmlDownloader, HttpHtmlDownloader>();
			services.AddScoped<IDownloadSchedulingService, DownloadSchedulingService>();
			services.AddScoped<IScrapingSeeder, ScrapingSeeder>();
			services.AddScoped<ITransformationService, TransformationService>();
			return services;
		}

		private static IServiceCollection AddRepositories(IServiceCollection services)
		{
			services.AddScoped<IScrapingRepository, ScrapingRepository>();
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
