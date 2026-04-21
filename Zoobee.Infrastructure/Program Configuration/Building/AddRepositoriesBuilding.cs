using Microsoft.Extensions.DependencyInjection;
using Zoobee.Application.Interfaces.Repositories.Catalog;
using Zoobee.Application.Interfaces.Repositories.Environment.Geography;
using Zoobee.Application.Interfaces.Repositories.Environment.Manufactures;
using Zoobee.Application.Interfaces.Repositories.Environment.Pets;
using Zoobee.Application.Interfaces.Repositories.IRepositoryBase;
using Zoobee.Application.Interfaces.Repositories.MediaStorage;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Application.Interfaces.Repositories.Sellings;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environment;
using Zoobee.Domain.DataEntities.Catalog.Reviews;
using Zoobee.Domain.DataEntities.Catalog.Tags;
using Zoobee.Domain.DataEntities.Environment.Creators;
using Zoobee.Domain.DataEntities.Environment.Geography;
using Zoobee.Domain.DataEntities.Environment.Manufactures;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.Repositoties.Catalog;
using Zoobee.Infrastructure.Repositoties.Environment.Geography;
using Zoobee.Domain.DataEntities.SellingsInformation;
using Zoobee.Infrastructure.Repositoties.Environment.Manufactures;
using Zoobee.Infrastructure.Repositoties.Environment.Pets;
using Zoobee.Infrastructure.Repositoties.MediaStorage;
using Zoobee.Infrastructure.Repositoties.Products;
using Zoobee.Infrastructure.Repositoties.Sellings;
using Zoobee.Infrastructure.Repositoties.UnitsOfWork;
using Zoobee.Infrastructure.Repositoties.UnitsOfWork.Environment;

namespace Zoobee.Infrastructure.ServiceCollectionExtensions
{
	public static class AddRepositoriesBuilding
	{
		public static void AddRepositories(this IServiceCollection services)
		{
			AddUnitsOfWork(services);

			//Media Storage
			services.AddScoped<IMediaFileRepository, MediaFileRepository>();
			services.AddScoped<IFileStorageRepository, FileStorageRepository>();

			//Products repositories
			services.AddScoped<IBaseProductsRepository, BaseProductsRepository>();
			services.AddScoped<IFoodProductsRepository, FoodProductRepository>();
			services.AddScoped<IRepositoryBase<FoodProductEntity>, FoodProductRepository>();
			services.AddScoped<IToiletProductsRepository, ToiletProductRepository>();
			services.AddScoped<IRepositoryBase<ToiletProductEntity>, ToiletProductRepository>();

			//Other 
			//Добавлять на основной интерфейс и на IRepositoryBase, шаблонный функционал, а в некоторых - расширенный
			services.AddScoped<IPetKindsRepository, PetKindsRepository>();
			services.AddScoped<IRepositoryBase<PetKindEntity>, PetKindsRepository>();

			services.AddScoped<ILocationsRepository, LocationRepository>();
			services.AddScoped<IRepositoryBase<LocationEntity>, LocationRepository>();

			services.AddScoped<IReviewsRepository, ReviewsRepository>();
			services.AddScoped<IRepositoryBase<ReviewEntity>, ReviewsRepository>();

			services.AddScoped<ITagsRepository, TagsRepository>();
			services.AddScoped<IRepositoryBase<TagEntity>, TagsRepository>();

			services.AddScoped<IDeliveryAreaRepository, DeliveryAreaRepository>();
			services.AddScoped<IRepositoryBase<DeliveryAreaEntity>, DeliveryAreaRepository>();

			services.AddScoped<IBrandsRepository, BrandRepository>();
			services.AddScoped<IRepositoryBase<BrandEntity>, BrandRepository>();

			services.AddScoped<ICreatorCountriesRepository, CreatorCountriesRepository>();
			services.AddScoped<IRepositoryBase<CreatorCountryEntity>, CreatorCountriesRepository>();

			services.AddScoped<ICreatorCompaniesRepository, CreatorCompanyRepository>();
			services.AddScoped<IRepositoryBase<CreatorCompanyEntity>, CreatorCompanyRepository>();

			services.AddScoped<IProductLineupRepository, ProductLineupsRepository>();
			services.AddScoped<IRepositoryBase<ProductLineupEntity>, ProductLineupsRepository>();

			services.AddScoped<ISellerCompanyRepository, SellerCompaniesRepository>();
			services.AddScoped<IRepositoryBase<SellerCompanyEntity>, SellerCompaniesRepository>();

			services.AddScoped<IZooStoresRepository, ZooStoresRepository>();
			services.AddScoped<IRepositoryBase<ZooStoreEntity>, ZooStoresRepository>();

			services.AddScoped<IPetKindsRepository, PetKindsRepository>();
			services.AddScoped<IRepositoryBase<PetKindEntity>, PetKindsRepository>();

			services.AddScoped<IDeliveryOptionsRepository, DeliveryOptionsRepository>();
			services.AddScoped<IRepositoryBase<DeliveryOptionEntity>, DeliveryOptionsRepository>();

			services.AddScoped<ISelfPickupOptionsRepository, SelfPickupOptionRepository>();
			services.AddScoped<IRepositoryBase<SelfPickupOptionEntity>, SelfPickupOptionRepository>();

			services.AddScoped<ISellingSlotsRepository, SellingSlotsRepository>();
			services.AddScoped<IRepositoryBase<SellingSlotEntity>, SellingSlotsRepository>();
		}

		private static void AddUnitsOfWork(IServiceCollection services)
		{
			services.AddScoped<IEnvironmentDataUnitOfWork, EnvironmentDataUnitOfWork>();
			services.AddScoped<IManufacturesUnitOfWork, ManufacturesUnitOfWork>();
			services.AddScoped<IGeographyDataUnitOfWork, GeographyDataUnitOfWork>();
			services.AddScoped<IPetsDataUnitOfWork, PetsDataUnitOfWork>();
			services.AddScoped<IProductsUnitOfWork, ProductsUnitOfWork>();
		}
	}
}
