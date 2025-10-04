using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Environment.Manufactures;

namespace Zoobee.Infrastructure.Repositoties.Environtment.Manufactures
{
	public class ProductLineupsRepository : RepositoryBase, IProductLineupRepository
	{
		public ProductLineupsRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<ProductLineupEntity> GetAll() => dbContext.ProductsLineups;
		public async Task<OperationResult<Guid>> CreateAsync(ProductLineupEntity newProductLineupEntity)
		{
			newProductLineupEntity.NormalizedLineupName = NormalizeString(newProductLineupEntity.LineupName);
			var res = await dbContext.ProductsLineups.AddAsync(newProductLineupEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.ProductsLineups.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(ProductLineupEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.ProductsLineups.ProductsLineupNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public ProductLineupEntity Get(Guid Id) => dbContext.ProductsLineups.FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(ProductLineupEntity productTypeToUpdate, Action<ProductLineupEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.ProductsLineups.ProductsLineupNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id) => dbContext.ProductsLineups.Any(e => e.Id == Id);

		public bool IsEntityExists(string brandName, string lineupName)
			=> dbContext.ProductsLineups
			.Any(e => e.Brand.NormalizedBrandName == NormalizeString(brandName)
				&& e.NormalizedLineupName == NormalizeString(lineupName));

		public ProductLineupEntity Get(string brandName, string lineupName)
			=> dbContext.ProductsLineups
			.FirstOrDefault(e => e.Brand.NormalizedBrandName == NormalizeString(brandName)
				&& e.NormalizedLineupName == NormalizeString(lineupName));

	}
}