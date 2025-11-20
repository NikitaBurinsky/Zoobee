using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Environment.Manufactures;

namespace Zoobee.Infrastructure.Repositoties.Environtment.Manufactures
{
	public class ZooStoresRepository : RepositoryBase, IZooStoresRepository
	{
		public ZooStoresRepository(ZoobeeAppDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<ZooStoreEntity> GetAll() => dbContext.ZooStores;
		public async Task<OperationResult<Guid>> CreateAsync(ZooStoreEntity newZooStoreEntity)
		{
			if (string.IsNullOrEmpty(newZooStoreEntity.Name))
				newZooStoreEntity.Name = newZooStoreEntity.SellerCompany.CompanyName;
			newZooStoreEntity.NormalizedName = NormalizeString(newZooStoreEntity.Name);
			var res = await dbContext.ZooStores.AddAsync(newZooStoreEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.ZooStores.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(ZooStoreEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.ZooStores.ZooStoreNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public ZooStoreEntity Get(Guid Id) => dbContext.ZooStores.FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(ZooStoreEntity productTypeToUpdate, Action<ZooStoreEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.ZooStores.ZooStoreNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id) => dbContext.ZooStores.Any(e => e.Id == Id);
	}
}