using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.FileStorage;
using Zoobee.Core.Errors;
using Zoobee.Domain.DataEntities.Media;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Zoobee.Domain;
using Zoobee.Application.Interfaces.Services.MediaStorage;
/// <summary>
/// TODO Описание
/// </summary>
public class AcceptableMediaTypesConfig : Dictionary<string, MediaType> { }
public class MediaFilesSizesBytesConfig : Dictionary<MediaType, uint> { }
public class MediaStorageService : IMediaStorageService
//TODO Ограничение на размер файлов
{
	public string _StorageFolderPath { get; }
	public IStringLocalizer<Errors> _localizer { get; }
	public IFileStorageRepository _fileStorageRepository { get; set; }
	public IMediaFileRepository _mediaFileRepository { get; set; }
	public Dictionary<string, MediaType> _acceptableMediaTypes { get; set; }
	public Dictionary<MediaType, uint> _maxMediaSizeBytes { get; set; }
	public MediaStorageService(IHostEnvironment environment, IStringLocalizer<Errors> localizer,
		IConfiguration configuration,
		IFileStorageRepository fileStorage,
		IMediaFileRepository mediaFiles,
		IOptions<AcceptableMediaTypesConfig> amtConfig,
		IOptions<MediaFilesSizesBytesConfig> mfsbConfig)
	{
		_fileStorageRepository = fileStorage;
		_mediaFileRepository = mediaFiles;
		_localizer = localizer;
		_StorageFolderPath = environment.ContentRootPath + configuration["FileStorage:RootSubFolder"] + "/";
		_acceptableMediaTypes = amtConfig.Value;
		_maxMediaSizeBytes = mfsbConfig.Value;
	}
	public async Task<OperationResult<MediaFileEntity>> SaveUploadedFileAsync(Stream file, string filename)
	{
		string fileExtension = Path.GetExtension(filename);
		string newFileName = Guid.NewGuid().ToString() + fileExtension;
		using var memoryStream = new MemoryStream();
		await file.CopyToAsync(memoryStream);
		memoryStream.Position = 0;

		MediaType mediaType = GetMediaType(fileExtension);
		if (!IsOkFileSize(file.Length, mediaType))
			return OperationResult<MediaFileEntity>.Error(_localizer["Error.MediaStorage.FileIsTooLarge"], HttpStatusCode.BadRequest);
		if (mediaType == MediaType.NONE)
			return OperationResult<MediaFileEntity>.Error(_localizer["Error.MediaStorage.UnsupportedMediaExtentions"], HttpStatusCode.BadRequest);
		var writeFileResult = await _fileStorageRepository.WriteFileAsync(memoryStream, newFileName, mediaType, mediaType.ToString(), _StorageFolderPath);
		if (writeFileResult.Succeeded)
		{
			var mediaFileRes = await _mediaFileRepository.AddMediaFile(writeFileResult.Returns);
			await _mediaFileRepository.SaveChangesAsync();
			if (mediaFileRes.Succeeded)
			{
				var Entity = await _mediaFileRepository.GetMediaFileAsync(mediaFileRes.Returns);
				return Entity != null ?
						OperationResult<MediaFileEntity>.Success(Entity) :
						OperationResult<MediaFileEntity>.Error(_localizer["Error.MediaStorage.WriteDbError"], HttpStatusCode.InternalServerError);
			}
			return OperationResult<MediaFileEntity>.Error(mediaFileRes.Message, mediaFileRes.ErrCode);
		}
		return writeFileResult;
	}

	public bool IsOkFileSize(long filesize, MediaType mediaType)
	{
		return _maxMediaSizeBytes[mediaType] <= filesize;
	}
	public MediaType GetMediaType(string extension)
	{
		var acceptFound = _acceptableMediaTypes.TryGetValue(extension, out var mediaType);
		return acceptFound ? mediaType : MediaType.NONE;
	}
	public async Task<OperationResult> DeleteMediaAsync(string filename)
	{
		var mediaFile = await _mediaFileRepository.GetMediaFileAsync(filename);
		if (mediaFile == null)
			return OperationResult.Error(_localizer["Error.MediaStorage.MediaFileNotFound"], HttpStatusCode.NotFound);
		if (_fileStorageRepository.IsFileExists(filename))
		{
			var res = await _fileStorageRepository.DeleteFileAsync(Path.Combine(_StorageFolderPath, mediaFile.FileLocalURL));
			await _mediaFileRepository.SaveChangesAsync();
			return res;
		}
		return OperationResult.Error(_localizer["Error.MediaStorage.FileNotFound"], HttpStatusCode.InternalServerError);
	}

	public bool IsMediaExists(string MediaUri)
		=> File.Exists(_StorageFolderPath + MediaUri);
}