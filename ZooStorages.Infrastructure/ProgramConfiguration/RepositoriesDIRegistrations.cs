using Microsoft.Extensions.DependencyInjection;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Infrastructure.Repositoties.Products;
using ZooStores.Infrastructure.Repositoties;
using ZooStorages.Infrastructure.Repositoties;
using ZooStorages.Application.Interfaces.Repositories.FileStorage;
using ZooStorages.Infrastructure.Repositoties.MediaStorage;

namespace ZooStorages.Infrastructure.ServiceCollectionExtensions
{
	public static class RepositoriesDIRegistrations
	{
		public static void AddRepositories(this IServiceCollection services)
		{
			services.AddScoped<IProductsRepository, ProductsRepository>();
			services.AddScoped<ICompaniesRepository, CompaniesRepository>();
			services.AddScoped<IStoresRepository, StoresRepository>();
			services.AddScoped<IProductSellingSlotsRepository, ProductsSellingSlotsRepository>();
			services.AddScoped<IProductTypisationRepository, ProductsTypisationRepository>();
			services.AddScoped<IPetKindsRepository, PetKindsRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IMediaFileRepository, MediaFileRepository>();
			services.AddScoped<IFileStorageRepository, FileStorageRepository>();
			services.AddScoped<ITagsRepository, TagsRepository>();
			services.AddScoped<IDynamicAttributesRepository, DynamicAttributesRepository>();
		}
	}
}
