using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Application.Interfaces.Repositories.FileStorage;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Media;
using ZooStores.Infrastructure.Repositoties;

namespace ZooStorages.Infrastructure.Repositoties.MediaStorage
{
	public class MediaFileRepository : RepositoryBase, IMediaFileRepository
	{
		public MediaFileRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{}

		public IQueryable<MediaFileEntity> MediaFiles => dbContext.MediaFiles;

		public async Task<OperationResult<Guid>> AddMediaFile(MediaFileEntity entity)
		{
			var res = dbContext.MediaFiles.Add(entity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.MediaFiles.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public async Task<OperationResult> DeleteMediaFile(MediaFileEntity entity)
		{
			if (dbContext.MediaFiles.Any(e => e.FileLocalURL == entity.FileLocalURL))
			{
				dbContext.MediaFiles.Remove(entity);
				return OperationResult.Success();
			}
			else
				return OperationResult.Error(localizer["Error.MediaFile.MediaFileNotFound"], HttpStatusCode.NotFound);
		}

		public async Task<MediaFileEntity> GetMediaFileAsync(Guid Id)
			=> dbContext.MediaFiles.FirstOrDefault(MediaFiles => MediaFiles.Id == Id);
		public async Task<MediaFileEntity> GetMediaFileAsync(string filename)
			=> dbContext.MediaFiles.FirstOrDefault(e => e.FileName == filename);

		public async Task<OperationResult> UpdateMediaFile(MediaFileEntity entity, Action<MediaFileEntity> action)
		{
			var res = dbContext.Update(entity);
			if (res == null)
				return OperationResult.Error(localizer["Error.MediaFile.UpdateError"], HttpStatusCode.InternalServerError);
			action(res.Entity);
			return OperationResult.Success();
		}
	}
}
