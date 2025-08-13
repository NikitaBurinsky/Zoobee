using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Core;
using ZooStorages.Domain.DataEntities.Products.Components.Tags;

namespace ZooStorages.Application.Interfaces.Repositories
{
	public interface ITagsRepository : IRepositoryBase
	{
		public IQueryable<TagEntity> Tags { get; }
		public Task<OperationResult<Guid>> CreateTagAsync(TagEntity tag);
		public Task<OperationResult> RenameTagAsync(TagEntity tagToUpdate, string newName);
		public Task<TagEntity> GetTagAsync(string Name);
		public Task<TagEntity> GetTagAsync(Guid Id);
		public Task<OperationResult> DeleteTagAsync(TagEntity tag);
		public bool IsTagExists(string Name);
		public bool IsTagExists(Guid Id);
	}
}
