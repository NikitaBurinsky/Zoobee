using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Catalog.Reviews;
using Zoobee.Infrastructure.Repositoties;

namespace Zoobee.Application.Interfaces.Repositories.Catalog
{
	public class ReviewsRepository : RepositoryBase, IReviewsRepository
	{
		public ReviewsRepository(ZoobeeAppDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<ReviewEntity> GetAll() => dbContext.Reviews;
		public async Task<OperationResult<Guid>> CreateAsync(ReviewEntity newReviewEntity)
		{
			var res = await dbContext.Reviews.AddAsync(newReviewEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.Reviews.WriteDbError"], HttpStatusCode.InternalServerError);
			var productUpdate = dbContext.Update(res.Entity.ReviewedProduct);
			productUpdate.Entity.AverageRating = productUpdate.Entity.Reviews.Average(x => x.Rating);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(ReviewEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.Reviews.ReviewNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public ReviewEntity Get(Guid Id) => dbContext.Reviews.FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(ReviewEntity productTypeToUpdate, Action<ReviewEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.Reviews.ReviewNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}
		public bool IsEntityExists(Guid Id) => dbContext.Reviews.Any(e => e.Id == Id);
	}
}