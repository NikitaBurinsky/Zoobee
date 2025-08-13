using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Core;
using ZooStorages.Domain.DataEntities.Media;

namespace ZooStorages.Application.Interfaces.Repositories.FileStorage
{
	/// <summary>
	/// Репозиторий работы с сущностями, хранящими данные о медиа-файлах
	/// </summary>
	public interface IMediaFileRepository : IRepositoryBase
	{
		public IQueryable<MediaFileEntity> MediaFiles { get; }
		public Task<OperationResult<Guid>> AddMediaFile(MediaFileEntity entity);
		public Task<MediaFileEntity> GetMediaFileAsync(Guid Id);
		public Task<MediaFileEntity> GetMediaFileAsync(string filename);
		public Task<OperationResult> DeleteMediaFile(MediaFileEntity entity);
		public Task<OperationResult> UpdateMediaFile(MediaFileEntity entity, Action<MediaFileEntity> action);
	}
}
