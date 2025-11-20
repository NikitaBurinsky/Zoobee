using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Sellings;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.Repositoties.Sellings
{
	public class SellingSlotsRepository : RepositoryBase, ISellingSlotsRepository
	{

		public SellingSlotsRepository(ZoobeeAppDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<SellingSlotEntity> GetAll() => dbContext.SellingSlots;
		public async Task<OperationResult<Guid>> CreateAsync(SellingSlotEntity newSellingSlotEntity)
		{
			var res = await dbContext.SellingSlots.AddAsync(newSellingSlotEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.SellingsSlots.WriteDbError"], HttpStatusCode.InternalServerError);
			var productUpdate = dbContext.Update(res.Entity.Product);
			productUpdate.Entity.MaxPrice = productUpdate.Entity.SellingSlots.Select(r => r.ResultPrice).Max();
			productUpdate.Entity.MinPrice = productUpdate.Entity.SellingSlots.Select(r => r.ResultPrice).Min();

			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(SellingSlotEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.SellingSlots.SellingSlotNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public SellingSlotEntity Get(Guid Id) => dbContext.SellingSlots.FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(SellingSlotEntity productTypeToUpdate, Action<SellingSlotEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.SellingSlots.SellingSlotNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id) => dbContext.SellingSlots.Any(e => e.Id == Id);
	}
}