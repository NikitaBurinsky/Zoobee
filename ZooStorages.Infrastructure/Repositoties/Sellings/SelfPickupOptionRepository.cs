using Microsoft.Extensions.Localization;
using System;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Catalog;
using Zoobee.Application.Interfaces.Repositories.Sellings;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Catalog.Reviews;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Domain.DataEntities.SellingsInformation;
using Zoobee.Infrastructure.Repositoties;
using Zoobee.Infrastructure.Repositoties;

namespace Zoobee.Infrastructure.Repositoties.Sellings
{
    public class SelfPickupOptionRepository : RepositoryBase, ISelfPickupOptionsRepository
	{
		public SelfPickupOptionRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<SelfPickupOptionEntity> GetAll() => dbContext.SelfPickupOptions;
		public async Task<OperationResult<Guid>> CreateAsync(SelfPickupOptionEntity newSelfPickupOptionEntity)
		{
			var res = await dbContext.SelfPickupOptions.AddAsync(newSelfPickupOptionEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.SelfPickupOptions.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(SelfPickupOptionEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.SelfPickupOptions.SelfPickupOptionNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public SelfPickupOptionEntity Get(Guid Id) => dbContext.SelfPickupOptions.FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(SelfPickupOptionEntity productTypeToUpdate, Action<SelfPickupOptionEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.SelfPickupOptions.SelfPickupOptionNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id) => dbContext.SelfPickupOptions.Any(e => e.Id == Id);
	}
}