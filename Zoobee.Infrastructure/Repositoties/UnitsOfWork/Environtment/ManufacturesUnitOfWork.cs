using Zoobee.Application.Interfaces.Repositories.Environment.Manufactures;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environment;

namespace Zoobee.Infrastructure.Repositoties.UnitsOfWork.Environment
{
	public class ManufacturesUnitOfWork : IManufacturesUnitOfWork
	{
		public ManufacturesUnitOfWork(IBrandsRepository brandsRepository,
			ICreatorCompaniesRepository creatorCompaniesRepository,
			ICreatorCountriesRepository creatorCountryRepository,
			IProductLineupRepository productLineupRepository,
			ISellerCompanyRepository sellerCompanyRepository,
			IZooStoresRepository zooStoresRepository)
		{
			BrandsRepository = brandsRepository;
			CreatorCompaniesRepository = creatorCompaniesRepository;
			CreatorCountryRepository = creatorCountryRepository;
			ProductLineupRepository = productLineupRepository;
			SellerCompanyRepository = sellerCompanyRepository;
			ZooStoresRepository = zooStoresRepository;
		}

		public IBrandsRepository BrandsRepository { get; }
		public ICreatorCompaniesRepository CreatorCompaniesRepository { get; }

		public ICreatorCountriesRepository CreatorCountryRepository { get; }

		public IProductLineupRepository ProductLineupRepository { get; }

		public ISellerCompanyRepository SellerCompanyRepository { get; }

		public IZooStoresRepository ZooStoresRepository { get; }
	}
}
