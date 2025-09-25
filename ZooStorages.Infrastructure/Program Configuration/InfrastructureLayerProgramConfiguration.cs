using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zoobee.Application.Interfaces.Services.EnvirontmentDataSeeding;
using Zoobee.Application.Interfaces.Services.GeoServices;
using Zoobee.Application.Interfaces.Services.GeoServices.GeoLocationService;
using Zoobee.Application.Interfaces.Services.GeoServices.GeoLocationService.GeoCoderApiClient;
using Zoobee.Application.Interfaces.Services.MediaStorage;
using Zoobee.Domain.DataEntities.Identity.Role;
using Zoobee.Domain.DataEntities.Identity.Users;
using Zoobee.Infrastructure.Repositoties;
using Zoobee.Infrastructure.Services.EnvirontmetnDataSeeding;
using Zoobee.Infrastructure.Services.GeoServices.CountryBordersService;
using Zoobee.Infrastructure.Services.GeoServices.GeoLocationService;
using Zoobee.Infrastructure.Services.GeoServices.GeoLocationService.GeoCoderApiClient;

namespace Zoobee.Application.ServiceCollectionExtensions
{
	public static class InfrastructureLayerDIRegistrations
	{
		public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpClient();

			services.AddDbContext<ZooStoresDbContext>(o =>
			{
				o.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
					b =>
					{
						b.MigrationsAssembly("Zoobee.Web");
					});
			});
			services.Configure<AcceptableMediaTypesConfig>(configuration.GetSection("FileStorage:AcceptableMediaExtensions"));
			services.Configure<MediaFilesSizesBytesConfig>(configuration.GetSection("FileStorage:MaxFileSizes"));
			//TEST
			services.AddIdentity<BaseApplicationUser, ApplicationRole>(o =>
			{
				o.Password.RequireNonAlphanumeric = false;
				o.Password.RequiredLength = 5;
				o.Password.RequireDigit = false;
				o.Password.RequireUppercase = false;
				o.Password.RequireLowercase = false;
			})
			.AddEntityFrameworkStores<ZooStoresDbContext>();
		}

		public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IGeoCoderApiClient, GeoCoderApiClient>();
			services.AddScoped<IGeoLocationService, GeoLocationService>();
			services.AddScoped<IMediaStorageService, MediaStorageService>();
			services.AddScoped<ICountryBorderService, CountryBorderService>();
			services.AddScoped<IEnvirontmentDataSeedingService, EnvirontmentDataSeedingService>();
		}
	}
}
