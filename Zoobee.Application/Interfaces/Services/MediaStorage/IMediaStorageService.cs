using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Media;

namespace Zoobee.Application.Interfaces.Services.MediaStorage
{

	/// <summary>
	/// Сервис работы с загружаемыми медиа
	/// Обьединяет работу репозиториев хранения файлов и MediaType, 
	/// предоставляя интерфейс загрузки, поиска и работы с медиа, хранящимися на сервере
	/// </summary>
	public interface IMediaStorageService
	{
		public Task<OperationResult<MediaFileEntity>> SaveUploadedFileAsync(Stream file, string filename);
		public Task<OperationResult> DeleteMediaAsync(string MediaUri);
		public bool IsMediaExists(string MediaUri);
	}
}
