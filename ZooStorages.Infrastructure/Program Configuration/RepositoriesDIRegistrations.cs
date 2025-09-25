using Microsoft.Extensions.DependencyInjection;
using Zoobee.Application.Interfaces.Repositories;
using Zoobee.Infrastructure.Repositoties;
using Zoobee.Infrastructure.Repositoties;
using Zoobee.Application.Interfaces.Repositories.FileStorage;
using Zoobee.Infrastructure.Repositoties.MediaStorage;
using Zoobee.Application.Interfaces.Repositories.Sellings;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Application.Interfaces.Repositories.Environtment.Pets;
using Zoobee.Infrastructure.Repositoties.Environtment.Pets;
using Zoobee.Application.Interfaces.Repositories.Environtment.Geography;
using Zoobee.Infrastructure.Repositoties.Environtment.Geography;
using Zoobee.Application.Interfaces.Repositories.Catalog;
using Zoobee.Infrastructure.Repositoties.Catalog;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Infrastructure.Repositoties.Environtment.Manufactures;
using Zoobee.Infrastructure.Repositoties.Sellings;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environment;
using Zoobee.Infrastructure.Repositoties.UnitsOfWork.Environtment;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork;
using Zoobee.Infrastructure.Repositoties.UnitsOfWork;
using Zoobee.Infrastructure.Repositoties.Products;

namespace Zoobee.Infrastructure.ServiceCollectionExtensions
{
    public static class RepositoriesDIRegistrations
	{
		public static void AddRepositories(this IServiceCollection services)
		{
			services.AddScoped<IMediaFileRepository, MediaFileRepository>();
			services.AddScoped<IFileStorageRepository, FileStorageRepository>();

			services.AddScoped<IBaseProductsRepository, BaseProductsRepository>();
			services.AddScoped<IFoodProductsRepository, FoodProductRepository>();
			services.AddScoped<IToiletProductsRepository, ToiletProductRepository>();
			services.AddScoped<IPetKindsRepository, PetKindsRepository>();
			services.AddScoped<ILocationsRepository, LocationRepository>();
			services.AddScoped<IReviewsRepository, ReviewsRepository>();
			services.AddScoped<ITagsRepository, TagsRepository>();
			services.AddScoped<IDeliveryAreaRepository, DeliveryAreaRepository>();
			services.AddScoped<IBrandsRepository, BrandRepository>();
			services.AddScoped<ICreatorCountriesRepository, CreatorCountriesRepository>();
			services.AddScoped<ICreatorCompaniesRepository, CreatorCompanyRepository>();
			services.AddScoped<IProductLineupRepository, ProductLineupsRepository>();
			services.AddScoped<ISellerCompanyRepository, SellerCompaniesRepository>();
			services.AddScoped<IZooStoresRepository, ZooStoresRepository>();
			services.AddScoped<IPetKindsRepository, PetKindsRepository>();
			services.AddScoped<IDeliveryOptionsRepository, DeliveryOptionsRepository>();
			services.AddScoped<ISelfPickupOptionsRepository, SelfPickupOptionRepository>();
			services.AddScoped<ISellingSlotsRepository, SellingSlotsRepository>();
			AddUnitsOfWork(services);
		}

		private static void AddUnitsOfWork(IServiceCollection services)
		{
			services.AddScoped<IEnvirontmentDataUnitOfWork, EnvirontmentDataUnitOfWork>();
			services.AddScoped<IManufacturesUnitOfWork, ManufacturesUnitOfWork>();
			services.AddScoped<IGeographyDataUnitOfWork, GeographyDataUnitOfWork>();
			services.AddScoped<IPetsDataUnitOfWork, PetsDataUnitOfWork>();
			services.AddScoped<IProductsUnitOfWork, ProductsUnitOfWork>();
		}
	}
}
