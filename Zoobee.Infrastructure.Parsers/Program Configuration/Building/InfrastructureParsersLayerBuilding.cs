#define TEST
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zoobee.Infrastructure.Parsers.Host;
using Zoobee.Infrastructure.Parsers.Interfaces.Pipelines.Scraping.Parsing;
using Zoobee.Infrastructure.Parsers.Interfaces.Pipelines.Scraping.ParsingPipeline;
using Zoobee.Infrastructure.Parsers.Interfaces.Pipelines.Scraping.ParsingQueue;
using Zoobee.Infrastructure.Parsers.Interfaces.Repositories.IParsedProductsRepository;
using Zoobee.Infrastructure.Parsers.Interfaces.Repositories.IParsersDbContext;
using Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsersDbContext;
using Zoobee.Infrastructure.Parsers.ParsedDataStorage.Repositories;
using Zoobee.Infrastructure.Parsers.ParsedDataStorage.Repositories.Zoobazar;
using Zoobee.Infrastructure.Parsers.Pipelines.Scraping.Parsers.Zoobazar;
using Zoobee.Infrastructure.Parsers.Pipelines.Scraping.Parsers.Zoobazar.Client;
using Zoobee.Infrastructure.Parsers.Pipelines.Scraping.ParsingPipelineService;
using Zoobee.Infrastructure.Parsers.Pipelines.Scraping.ParsingQueue;
using Zoobee.Infrastructure.Parsers.Storage.ParsedDataEntities.ParsedProductsModels.Zoobazar;

namespace Zoobee.Infrastructure.Parsers.Program_Configuration.Pipelines
{
	public static class AddParsersBuildingExtensions
	{
		public static IServiceCollection AddParsers(this IServiceCollection services, IConfiguration configuration, bool IsInMemoryDatabase = false)
		{
			AddDbContext(services, configuration, IsInMemoryDatabase);
			AddScrapingPipelinesServices(services);
			AddParsersMappingProfiles(services);
			
			AddZoobazarParserPipeline(services);

			services.AddHostedService<ZoobeeParsersWorker>();
			return services;
		}
		private static void AddParsersMappingProfiles(IServiceCollection services)
		{


		}

		private static void AddDbContext(IServiceCollection services, IConfiguration configuration, bool IsInMemoryDebug)
		{
			if (!IsInMemoryDebug)
			{
				string conStringParsers = configuration.GetConnectionString("ParsersDatabaseConnection");
				services.AddDbContext<ParsersDbContext>(o =>
				{
					o.UseNpgsql(conStringParsers,
						b =>
						{
							//TODO Перенести миграции из WEB'a в infrastructure
							b.MigrationsAssembly("Zoobee.Web");
						});
				});
			}
			else
			{
				services.AddDbContext<ParsersDbContext>(o =>
				{
					o.UseInMemoryDatabase("ParsersInMemoryDatabase");
				});
			}
			services.AddScoped<IParsersDbContext, ParsersDbContext>();
		}
		private static void AddScrapingPipelinesServices(IServiceCollection services)
		{
			services.AddScoped<IParsingQueueService, ParsingQueue>();
		}

		private static void AddZoobazarParserPipeline(IServiceCollection services)
		{
			services.AddScoped<ISiteProductParser<ZoobazarParsedProduct>, ProductParser_Zoobazar>();
			services.AddScoped<ISiteParsingPipelineService, SiteParsingPipelineService<ZoobazarParsedProduct>>();
			services.AddScoped<ZoobazarClient>();
			services.AddScoped<IParsedProductsRepository<ZoobazarParsedProduct>, ZoobazarParsedProductsRepository>();
		}







	}
}
