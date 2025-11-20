using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Catalog;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Catalog.Tags;

namespace Zoobee.Infrastructure.Repositoties.Catalog
{
	public class TagsRepository : RepositoryBase, ITagsRepository
	{
		public TagsRepository(ZoobeeAppDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<TagEntity> GetAll() => dbContext.Tags;
		public async Task<OperationResult<Guid>> CreateAsync(TagEntity newTagEntity)
		{
			newTagEntity.NormalizedTagName = NormalizeString(newTagEntity.TagName);
			var res = await dbContext.Tags.AddAsync(newTagEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.Tags.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(TagEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.Tags.TagNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public TagEntity Get(Guid Id) => dbContext.Tags.FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(TagEntity productTypeToUpdate, Action<TagEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.Tags.TagNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id) => dbContext.Tags.Any(e => e.Id == Id);
	}
}