using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;

namespace Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment
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
