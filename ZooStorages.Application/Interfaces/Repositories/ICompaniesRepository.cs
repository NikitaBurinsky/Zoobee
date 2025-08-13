using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Core;
using ZooStores.Application.DtoTypes.Companies;
using ZooStores.Application.DtoTypes.Products;

namespace ZooStorages.Application.Interfaces.Repositories
{
	public interface ICompaniesRepository : IRepositoryBase
	{
		IQueryable<CompanyEntity> Companies { get; }
		Task<OperationResult<Guid>> CreateCompanyAsync(CompanyEntity newCompany);
		Task<OperationResult> UpdateCompanyAsync(CompanyEntity updatedCompany, Action<CompanyEntity> action);
		Task<CompanyEntity> GetCompanyAsync(Guid id);
		Task<CompanyEntity> GetCompanyAsync(string companyName);
		Task<OperationResult> DeleteCompanyAsync(CompanyEntity newCompany);
		Task LoadCompanyStoresAsync(CompanyEntity company);
		Task LoadCompanyClinicsAsync(CompanyEntity company);
		bool IsCompanyExists(Guid id);
		bool IsCompanyExists(string companyName);
		Task<int> SaveChangesAsync();
	}
}	
