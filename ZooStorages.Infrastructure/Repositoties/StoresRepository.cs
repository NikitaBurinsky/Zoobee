using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Infrastructure.Repositoties;
using ZooStores.Application.DtoTypes.Companies;
using ZooStores.Application.DtoTypes.Stores;
namespace ZooStores.Infrastructure.Repositoties
{
	public class StoresRepository : RepositoryBase, IStoresRepository
	{
		public StoresRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }

		public IQueryable<VetStoreEntity> VetStores => dbContext.VetStores;

		public async Task<OperationResult<Guid>> CreateStoreAsync(VetStoreEntity newVetStore)
		{
			if (!dbContext.VetStores.Any(c => c.ownerCompanyId == newVetStore.ownerCompanyId))
				return OperationResult<Guid>.Error(localizer["Error.Stores.CompanyNotFound"], HttpStatusCode.NotFound);
			var res = dbContext.VetStores.AddAsync(newVetStore);
			return OperationResult<Guid>.Success(res.Result.Entity.Id);
		}

		public async Task<OperationResult> DeleteStoreWithProductsAsync(VetStoreEntity newVetStore)
		{
			if (IsStoreExists(newVetStore.Id))
			{
				foreach (var product in newVetStore.avaibabProducts)
					dbContext.SellingSlots.Remove(product);
				dbContext.Remove(newVetStore);
				return OperationResult.Success();
			}
			else
				return OperationResult.Error(localizer["Error.Stores.StoreNotFound"], HttpStatusCode.NotFound);
		}

		public async Task<VetStoreEntity> GetStoreAsync(Guid id) => await dbContext.VetStores.FindAsync(id);

		public async Task<VetStoreEntity> GetStoreAsync(CompanyEntity ownerCompany, string storeName)
			=> await dbContext.VetStores.FirstOrDefaultAsync(s =>
				s.ownerCompanyId == ownerCompany.Id && s.Name == storeName);

		public async Task<OperationResult<List<VetStoreEntity>>> GetStoresOfCompany(CompanyEntity company)
		{
			var entry = dbContext.Entry(company);
			if (entry == null)
				return OperationResult<List<VetStoreEntity>>.Error(localizer["Error.Stores.CompanyNotFound"], HttpStatusCode.NotFound);
			await entry.Collection(c => c.ownedStores).LoadAsync();
			return OperationResult<List<VetStoreEntity>>.Success(company.ownedStores.ToList());
		}

		public async Task<OperationResult<List<VetStoreEntity>>> GetStoresOfCompany(string companyName)
		{
			var company = await dbContext.Companies.FirstOrDefaultAsync(c => c.Name == companyName);
			if (company == null)
				return OperationResult<List<VetStoreEntity>>.Error(localizer["Error.Stores.CompanyNotFound"], HttpStatusCode.NotFound);
			var entry = dbContext.Entry(company);
			await entry.Collection(c => c.ownedStores).LoadAsync();
			return OperationResult<List<VetStoreEntity>>.Success(company.ownedStores.ToList());
		}

		public bool IsStoreExists(Guid Id)
			=> dbContext.VetStores.Any(e => e.Id == Id);

		public bool IsStoreExists(Guid CompanyId, string StoreName)
			=> dbContext.VetStores.Any(e => e.ownerCompanyId == CompanyId && e.Name == StoreName);

		public async Task<OperationResult> UpdateStoreAsync(VetStoreEntity updateVetStore, Action<VetStoreEntity> action)
		{
			var x = dbContext.Update(updateVetStore);
			if (x == null)
				return OperationResult.Error(localizer["Error.Stores.StoreNotFound"], HttpStatusCode.NotFound);
			action(x.Entity);
			return OperationResult.Success();
		}
	}
}
