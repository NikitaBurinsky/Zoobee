using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.Repositoties.Products
{
	public class BaseProductsRepository : RepositoryBase, IBaseProductsRepository
	{
		public BaseProductsRepository(ZoobeeAppDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }

		public IQueryable<BaseProductEntity> GetAll() => dbContext.Products;

		public bool IsEntityExists(Guid Id) => dbContext.Products.Any(p => p.Id == Id);

		public async Task<OperationResult<Guid>> CreateAsync(BaseProductEntity newFoodProductEntity)
		{
			newFoodProductEntity.NormalizedName = NormalizeString(newFoodProductEntity.Name);

			var res = await dbContext.Products.AddAsync(newFoodProductEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.FoodProducts.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(BaseProductEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.FoodProducts.FoodProductNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public BaseProductEntity Get(Guid Id) => GetAll().FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(BaseProductEntity productTypeToUpdate, Action<BaseProductEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.FoodProducts.FoodProductNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}
	}
}
