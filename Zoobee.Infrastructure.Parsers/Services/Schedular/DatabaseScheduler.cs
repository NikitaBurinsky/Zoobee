using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zoobee.Infrastructure.Parsers.Core.Configuration;
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Downloader;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Scheduling;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;

namespace Zoobee.Infrastructure.Parsers.Services.Scheduling
{

	public class DownloadSchedulingService : IDownloadSchedulingService
	{
		private readonly IScrapingRepository _repository;
		private readonly ILogger<DownloadSchedulingService> _logger;
		private readonly ScrapingOptions _options;

		public DownloadSchedulingService(
			IScrapingRepository repository,
			ILogger<DownloadSchedulingService> logger,
			IOptions<ScrapingOptions> options)
		{
			_repository = repository;
			_logger = logger;
			_options = options.Value;
		}

		public async Task<IEnumerable<ScrapingTask>> GetNextBatchAsync(int batchSize, CancellationToken ct)
		{
			return await _repository.GetPendingTasksAsync(batchSize, ct);
		}

		public async Task HandleDownloadResultAsync(ScrapingTask task, DownloadResult result, CancellationToken ct)
		{
			// 1. Создаем объект данных (историю)
			var dataEntry = new ScrapingData
			{
				Id = Guid.NewGuid(),
				ScrapingTaskId = task.Id,
				Content = result.IsSuccess ? result.Content : null, // При ошибке контент не храним (экономия)
				HttpStatusCode = result.StatusCode,
				ErrorMessage = result.IsSuccess ? null : result.ErrorMessage,
				Metadata = new Domain.DataEntities.Base.IEntityMetadata.EntityMetadata
				{
					CreatedAt = DateTime.UtcNow,
				}
			};

			// 2. Рассчитываем следующее время для ЗАДАЧИ
			if (result.IsSuccess)
			{
				// --- УСПЕХ ---
				// Сбрасываем счетчик попыток
				task.AttemptCount = 0;
				// Ставим в Pending, чтобы скачать снова через N часов
				task.Status = RawPageStatus.Pending;

				// Используем CustomFrequencyHours из задачи, если есть, иначе дефолт из конфига
				int hoursToWait = task.CustomFrequencyHours ?? _options.DefaultParsingFrequencyHours;
				task.NextTryAt = DateTime.UtcNow.AddHours(hoursToWait);

				// Для справки можно добавить поле LastSuccessAt в Task, если нужно
			}
			else
			{
				// --- ОШИБКА ---
				task.AttemptCount++;

				if (result.StatusCode == 404)
				{
					task.Status = RawPageStatus.NotFound;
					task.NextTryAt = DateTime.MaxValue;
				}
				else
				{
					// Логика Retry
					var delays = _options.RetryDelaysMinutes;
					if (task.AttemptCount >= delays.Length)
					{
						task.Status = RawPageStatus.Failed;
						task.NextTryAt = DateTime.MaxValue;
						_logger.LogError("Задача {Url} провалена.", task.Url);
					}
					else
					{
						int delayMin = delays[task.AttemptCount - 1];
						task.NextTryAt = DateTime.UtcNow.AddMinutes(delayMin);
						task.Status = RawPageStatus.Pending;
					}
				}
			}

			// 3. Сохраняем и Task (обновление), и Data (вставка)
			await _repository.SaveExecutionResultAsync(task, dataEntry, ct);
		}
	}
}