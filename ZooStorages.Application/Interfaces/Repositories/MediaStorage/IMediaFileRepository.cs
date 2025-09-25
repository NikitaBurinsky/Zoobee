using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Media;

namespace Zoobee.Application.Interfaces.Repositories.FileStorage
{
    /// <summary>
    /// Репозиторий работы с сущностями, хранящими данные о медиа-файлах
    /// </summary>
    public interface IMediaFileRepository 
	{
		public IQueryable<MediaFileEntity> MediaFiles { get; }
		public Task<OperationResult<Guid>> AddMediaFile(MediaFileEntity entity);
		public Task<MediaFileEntity> GetMediaFileAsync(Guid Id);
		public Task<MediaFileEntity> GetMediaFileAsync(string filename);
		public Task<OperationResult> DeleteMediaFile(MediaFileEntity entity);
		public Task<OperationResult> UpdateMediaFile(MediaFileEntity entity, Action<MediaFileEntity> action);
		public int SaveChanges();
		public Task<int> SaveChangesAsync();
	}
}
