using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment;

namespace Zoobee.Infrastructure.Repositoties.UnitsOfWork.Environtment
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

		public IBrandsRepository BrandsRepository {get;}
		public ICreatorCompaniesRepository CreatorCompaniesRepository {get;}

		public ICreatorCountriesRepository CreatorCountryRepository {get;}

		public IProductLineupRepository ProductLineupRepository {get;}

		public ISellerCompanyRepository SellerCompanyRepository {get;}

		public IZooStoresRepository ZooStoresRepository {get;}
	}
}
