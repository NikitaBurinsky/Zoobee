using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products.FoodProductEntity;

namespace Zoobee.Infrastructure.Repositoties.Products
{
	public class FoodProductRepository : RepositoryBase, IFoodProductsRepository
	{
		public FoodProductRepository(ZoobeeAppDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<FoodProductEntity> GetAll() => dbContext.Products.OfType<FoodProductEntity>();
		public async Task<OperationResult<Guid>> CreateAsync(FoodProductEntity newFoodProductEntity)
		{
			newFoodProductEntity.NormalizedName = NormalizeString(newFoodProductEntity.Name);

			var res = await dbContext.Products.AddAsync(newFoodProductEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.FoodProducts.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(FoodProductEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.FoodProducts.FoodProductNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public FoodProductEntity Get(Guid Id) => GetAll().FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(FoodProductEntity productTypeToUpdate, Action<FoodProductEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.FoodProducts.FoodProductNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id) => GetAll().Any(e => e.Id == Id);
	}
}