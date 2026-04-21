using Zoobee.Application.Interfaces.Repositories.Environment.Manufactures;

namespace Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environment
{
	public interface IManufacturesUnitOfWork
	{
		IBrandsRepository BrandsRepository { get; }
		ICreatorCompaniesRepository CreatorCompaniesRepository { get; }
		ICreatorCountriesRepository CreatorCountryRepository { get; }
		IProductLineupRepository ProductLineupRepository { get; }
		ISellerCompanyRepository SellerCompanyRepository { get; }
		IZooStoresRepository ZooStoresRepository { get; }
	}
}
