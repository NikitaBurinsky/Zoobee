using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.Interfaces.Repositories;
using Zoobee.Application.Interfaces.Repositories.FileStorage;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Media;
using Zoobee.Infrastructure.Repositoties;

namespace Zoobee.Infrastructure.Repositoties.MediaStorage
{
    public class MediaFileRepository : RepositoryBase, IMediaFileRepository
	{
		public MediaFileRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{}

		public IQueryable<MediaFileEntity> MediaFiles => dbContext.MediaFiles;

		public async Task<OperationResult<Guid>> AddMediaFile(MediaFileEntity Entity)
		{
			var res = dbContext.MediaFiles.Add(Entity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.MediaFiles.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public async Task<OperationResult> DeleteMediaFile(MediaFileEntity Entity)
		{
			if (dbContext.MediaFiles.Any(e => e.FileLocalURL == Entity.FileLocalURL))
			{
				dbContext.MediaFiles.Remove(Entity);
				return OperationResult.Success();
			}
			else
				return OperationResult.Error(localizer["Error.MediaFile.MediaFileNotFound"], HttpStatusCode.NotFound);
		}

		public async Task<MediaFileEntity> GetMediaFileAsync(Guid Id)
			=> dbContext.MediaFiles.FirstOrDefault(MediaFiles => MediaFiles.Id == Id);
		public async Task<MediaFileEntity> GetMediaFileAsync(string filename)
			=> dbContext.MediaFiles.FirstOrDefault(e => e.FileName == filename);

		public async Task<OperationResult> UpdateMediaFile(MediaFileEntity Entity, Action<MediaFileEntity> action)
		{
			var res = dbContext.Update(Entity);
			if (res == null)
				return OperationResult.Error(localizer["Error.MediaFile.UpdateError"], HttpStatusCode.InternalServerError);
			action(res.Entity);
			return OperationResult.Success();
		}
	}
}
