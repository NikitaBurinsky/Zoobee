using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZooStorages.Application.Interfaces;
using ZooStorages.Application.Interfaces.Services;
using ZooStorages.Application.Interfaces.Services.ProductsAdminitrator;
using ZooStorages.Domain.DataEntities.Identity;
using ZooStorages.Domain.DataEntities.Media;
using ZooStorages.Infrastructure.Services;
using ZooStores.Infrastructure.Repositoties;

namespace ZooStorages.Application.ServiceCollectionExtensions
{
	public static class InfrastructureLayerDIRegistrations
	{
		public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<ZooStoresDbContext>(o =>
			{
				o.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
					b =>
					{
					b.MigrationsAssembly("ZooStores.01_Web");
					b.UseNetTopologySuite();
			});
			});
			services.Configure<AcceptableMediaTypesConfig>(configuration.GetSection("FileStorage:AcceptableMediaExtensions"));
			services.Configure<MediaFilesSizesBytesConfig>(configuration.GetSection("FileStorage:MaxFileSizes"));

			//TEST
			services.AddIdentity<TestUser, IdentityRole>()
				.AddEntityFrameworkStores<ZooStoresDbContext>();
		}

		public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IMediaStorageService, MediaStorageService>();
			services.AddScoped<IProductsAdministratorService, ProductsAdministratorService>();
		}
}
}
