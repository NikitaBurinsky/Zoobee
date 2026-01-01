using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zoobee.Application.Interfaces.Services;
using Zoobee.Application.Interfaces.Services.EnvirontmentDataSeeding;
using Zoobee.Application.Interfaces.Services.GeoServices;
using Zoobee.Application.Interfaces.Services.GeoServices.GeoLocationService;
using Zoobee.Application.Interfaces.Services.GeoServices.GeoLocationService.GeoCoderApiClient;
using Zoobee.Application.Interfaces.Services.MediaStorage;
using Zoobee.Application.Interfaces.Services.Products.ProductsFinder;
using Zoobee.Application.Interfaces.Services.Products.ProductsStorage;
using Zoobee.Domain.DataEntities.Identity.Role;
using Zoobee.Domain.DataEntities.Identity.Users;
using Zoobee.Infrastructure.Repositoties;
using Zoobee.Infrastructure.Services.DtoMappingService;
using Zoobee.Infrastructure.Services.EnvirontmetnDataSeeding;
using Zoobee.Infrastructure.Services.GeoServices.CountryBordersService;
using Zoobee.Infrastructure.Services.GeoServices.GeoLocationService;
using Zoobee.Infrastructure.Services.GeoServices.GeoLocationService.GeoCoderApiClient;
using Zoobee.Infrastructure.Services.Products.ProductsFinder;
using Zoobee.Infrastructure.Services.Products.ProductsStorage;

namespace Zoobee.Application.ServiceCollectionExtensions
{
	public static class InfrastructureLayerDIRegistrations
	{
		public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, bool IsInMemoryDatabase = false)
		{
			services.AddHttpClient();

			AddDbContext(services, configuration, IsInMemoryDatabase);
			AddInfrastructureServices(services, configuration);
			AddConfigurationClasses(services, configuration);
			//TEST
			AddIdentitySystem(services);
		}

		private static void AddIdentitySystem(IServiceCollection services)
		{
			services.AddIdentity<BaseApplicationUser, ApplicationRole>(o =>
			{
				o.Password.RequireNonAlphanumeric = false;
				o.Password.RequiredLength = 5;
				o.Password.RequireDigit = false;
				o.Password.RequireUppercase = false;
				o.Password.RequireLowercase = false;
			})
			.AddEntityFrameworkStores<ZoobeeAppDbContext>();
		}
		private static void AddConfigurationClasses(IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<AcceptableMediaTypesConfig>(configuration.GetSection("FileStorage:AcceptableMediaExtensions"));
			services.Configure<MediaFilesSizesBytesConfig>(configuration.GetSection("FileStorage:MaxFileSizes"));
		}
		private static void AddDbContext(IServiceCollection services, IConfiguration configuration, bool IsInMemory)
		{
			if (IsInMemory)
				services.AddDbContext<ZoobeeAppDbContext>(o =>
				{
					o.UseInMemoryDatabase("ZoobeeDevelopmentIM");
				});
			else
				services.AddDbContext<ZoobeeAppDbContext>(o =>
				{
					o.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
						b =>
						{
							b.MigrationsAssembly("Zoobee.Web");
						});
				});
		}
		private static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IGeoCoderApiClient, GeoCoderApiClient>();
			services.AddScoped<IGeoLocationService, GeoLocationService>();
			services.AddScoped<IProductsStorageService, ProductsStorageService>();
			services.AddScoped<IMediaStorageService, MediaStorageService>();
			services.AddScoped<ICountryBorderService, CountryBorderService>();
			services.AddScoped<IEnvirontmentDataSeedingService, EnvirontmentDataSeedingService>();
			services.AddScoped<IProductsFinderService, ProductsFinderService>();
			services.AddScoped<IMappingService, DtoMappingService>();
		}
	}
}
