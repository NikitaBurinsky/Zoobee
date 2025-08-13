using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products.Components.Tags;
using ZooStores.Infrastructure.Repositoties;

namespace ZooStorages.Infrastructure.Repositoties
{
	public class TagsRepository : RepositoryBase, ITagsRepository
	{
		public TagsRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{}
		public IQueryable<TagEntity> Tags => dbContext.Tags;

		public async Task<OperationResult<Guid>> CreateTagAsync(TagEntity tag)
		{
			if (IsTagExists(tag.TagName))
				return OperationResult<Guid>.Error(localizer["Error.Tags.SimilarNameExists"], HttpStatusCode.BadRequest);
			var res = await dbContext.Tags.AddAsync(tag);
			return res != null ?
				OperationResult<Guid>.Success(res.Entity.Id) :
				OperationResult<Guid>.Error(localizer["Error.Tags.WriteDbError"], HttpStatusCode.InternalServerError);
		}
		public async Task<OperationResult> DeleteTagAsync(TagEntity tag)
		{
			var res = dbContext.Tags.Remove(tag);
			return res != null ?
				OperationResult.Success() :
				OperationResult.Error(localizer["Error.Tags.DeleteDbError"], HttpStatusCode.InternalServerError);
		}
		public async Task<TagEntity> GetTagAsync(string Name)
			=> dbContext.Tags.FirstOrDefault(e => e.TagName == Name);
		public async Task<TagEntity> GetTagAsync(Guid Id)
			=> dbContext.Tags.FirstOrDefault(e => e.Id == Id);
		public bool IsTagExists(string tagName)
			=> dbContext.Tags.Any(e => e.TagName == tagName);
		public bool IsTagExists(Guid Id)
			=> dbContext.Tags.Any(e => e.Id == Id);

		public async Task<OperationResult> RenameTagAsync(TagEntity tagToUpdate, string newName)
		{
			if (IsTagExists(newName))
				return OperationResult.Error(localizer["Error.Tags.SimilarNameExists"], HttpStatusCode.BadRequest);
			var res = dbContext.Tags.Update(tagToUpdate);
			if (res == null)
				return OperationResult.Error(localizer["Error.Tags.TagNotFound"], HttpStatusCode.InternalServerError);
			tagToUpdate.TagName = newName;
			return OperationResult.Success();
		}
	}
}
