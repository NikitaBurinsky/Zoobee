using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Media;

namespace Zoobee.Application.Interfaces.Repositories.MediaStorage
{
	/// <summary>
	/// Репозиторий хранения файлов на диске (не бд), c оперированием инфо о файлам в MediaFile
	/// </summary>
	public interface IFileStorageRepository
	{
		Task<OperationResult<MediaFileEntity>> WriteFileAsync(Stream fileStream, string fileName, MediaType mediaType, string localpath, string storagePath);
		Task<OperationResult> DeleteFileAsync(string path);
		Task<Stream> GetFileAsync(string path);
		bool IsFileExists(string path);
	}

}
