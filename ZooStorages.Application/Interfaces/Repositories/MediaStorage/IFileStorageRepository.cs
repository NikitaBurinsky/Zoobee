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
