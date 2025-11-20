using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Sellings;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.SellingsInformation;

namespace Zoobee.Infrastructure.Repositoties.Sellings
{
	public class DeliveryOptionsRepository : RepositoryBase, IDeliveryOptionsRepository
	{
		public DeliveryOptionsRepository(ZoobeeAppDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<DeliveryOptionEntity> GetAll() => dbContext.DeliveryOptions;
		public async Task<OperationResult<Guid>> CreateAsync(DeliveryOptionEntity newDeliveryOptionEntity)
		{
			var res = await dbContext.DeliveryOptions.AddAsync(newDeliveryOptionEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.DeliveryOptions.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(DeliveryOptionEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.DeliveryOptions.DeliveryOptionNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public DeliveryOptionEntity Get(Guid Id) => dbContext.DeliveryOptions.FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(DeliveryOptionEntity productTypeToUpdate, Action<DeliveryOptionEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.DeliveryOptions.DeliveryOptionNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id) => dbContext.DeliveryOptions.Any(e => e.Id == Id);


	}
}