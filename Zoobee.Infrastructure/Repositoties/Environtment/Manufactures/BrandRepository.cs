using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Environment.Creators;

namespace Zoobee.Infrastructure.Repositoties.Environtment.Manufactures
{
	public class BrandRepository : RepositoryBase, IBrandsRepository
	{
		public BrandRepository(ZoobeeAppDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<BrandEntity> GetAll() => dbContext.Brands;
		public async Task<OperationResult<Guid>> CreateAsync(BrandEntity newBrandEntity)
		{
			newBrandEntity.NormalizedBrandName = NormalizeString(newBrandEntity.BrandName);
			var res = await dbContext.Brands.AddAsync(newBrandEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.Brands.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(BrandEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.Brands.BrandNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public BrandEntity Get(Guid Id) => dbContext.Brands.FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(BrandEntity productTypeToUpdate, Action<BrandEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.Brands.BrandNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id) => dbContext.Brands.Any(e => e.Id == Id);

		public BrandEntity Get(string brandName)
			=> dbContext.Brands.FirstOrDefault(e => e.NormalizedBrandName == NormalizeString(brandName));

		public bool IsEntityExists(string brandName)
			=> dbContext.Brands.Any(e => e.NormalizedBrandName == NormalizeString(brandName));
	}
}