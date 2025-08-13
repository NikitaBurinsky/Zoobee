using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Core;
using ZooStores.Application.DtoTypes.Companies;
using ZooStores.Application.DtoTypes.Stores;

namespace ZooStorages.Application.Interfaces.Repositories
{
	public interface IStoresRepository : IRepositoryBase
	{
		IQueryable<VetStoreEntity> VetStores{ get; }
		public Task<OperationResult<Guid>> CreateStoreAsync(VetStoreEntity newVetStore);
		public Task<OperationResult> UpdateStoreAsync(VetStoreEntity updatedVetStore, Action<VetStoreEntity> action);
		public Task<VetStoreEntity> GetStoreAsync(Guid id);
		public Task<VetStoreEntity> GetStoreAsync(CompanyEntity ownerCompany, string storeName);
		public bool IsStoreExists(Guid Id);
		public bool IsStoreExists(Guid CompanyId, string StoreName);
		public Task<OperationResult> DeleteStoreWithProductsAsync(VetStoreEntity newVetStore);
		public Task<OperationResult<List<VetStoreEntity>>> GetStoresOfCompany(CompanyEntity companyEntity);
		public Task<OperationResult<List<VetStoreEntity>>> GetStoresOfCompany(string companyName);
		public Task<int> SaveChangesAsync();
	}
}
