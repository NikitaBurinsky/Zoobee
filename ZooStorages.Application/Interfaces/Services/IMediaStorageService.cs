using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Core;
using ZooStorages.Domain.DataEntities.Media;

namespace ZooStorages.Application.Interfaces.Services
{

	/// <summary>
	/// Сервис работы с загружаемыми медиа
	/// Обьединяет работу репозиториев хранения файлов и MediaType, 
	/// предоставляя интерфейс загрузки, поиска и работы с медиа, хранящимися на сервере
	/// </summary>
	public interface IMediaStorageService
	{
		public Task<OperationResult<MediaFileEntity>> SaveUploadedFileAsync(Stream file, string filename);
		public Task<OperationResult> DeleteMediaAsync(string filename);
		public bool IsMediaExists(string MediaUri);
	}
}
