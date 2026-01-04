using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.Repositoties.Products
{
	public class ToiletProductRepository : RepositoryBase, IToiletProductsRepository
	{
		public ToiletProductRepository(ZoobeeAppDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<ToiletProductEntity> GetAll() => dbContext.Products.OfType<ToiletProductEntity>();
		public async Task<OperationResult<Guid>> CreateAsync(ToiletProductEntity newToiletProductEntity)
		{
			newToiletProductEntity.NormalizedName = NormalizeString(newToiletProductEntity.Name);

			var res = await dbContext.Products.AddAsync(newToiletProductEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.ToiletProducts.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}
		public OperationResult Delete(ToiletProductEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.ToiletProducts.ToiletProductNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public ToiletProductEntity Get(Guid Id) => GetAll().FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(ToiletProductEntity productTypeToUpdate, Action<ToiletProductEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.ToiletProducts.ToiletProductNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id) => GetAll().Any(e => e.Id == Id);
	}
}