using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options; // <-- Добавляем для IOptions
using Zoobee.Infrastructure.Parsers.Core.Configuration; // <-- Для ScrapingOptions
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Downloader;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Scheduling;

namespace Zoobee.Infrastructure.Parsers.Workers
{
	public class ScrapingWorker : BackgroundService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger<ScrapingWorker> _logger;
		// Храним объект конфигурации для использования в цикле
		private readonly ScrapingOptions _options;

		// Константы удалены, их значения приходят из _options

		public ScrapingWorker(
			IServiceProvider serviceProvider,
			ILogger<ScrapingWorker> logger,
			IOptions<ScrapingOptions> options) // <-- Внедрение конфигурации
		{
			_serviceProvider = serviceProvider;
			_logger = logger;
			_options = options.Value; // Сохраняем значения
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Scraping Worker: ЗАПУЩЕН.");

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using (var scope = _serviceProvider.CreateScope())
					{
						var scheduler = scope.ServiceProvider.GetRequiredService<IDownloadSchedulingService>();
						var downloader = scope.ServiceProvider.GetRequiredService<IHtmlDownloader>();

						// 2. Получаем следующую задачу из очереди
						var batch = await scheduler.GetNextBatchAsync(1, stoppingToken);
						var task = batch.FirstOrDefault();

						if (task == null)
						{
							// Используем значение из конфигурации
							await Task.Delay(_options.NoTasksDelayMs, stoppingToken);
							continue;
						}

						_logger.LogInformation("Scraping Worker: Начало обработки {Url}", task.Url);

						// 3. Rate Limiting (Простая реализация)
						// Используем значение из конфигурации
						await Task.Delay(_options.RequestDelayMs, stoppingToken);

						// 4. Скачивание (Основная работа)
						DownloadResult result;
						try
						{
							result = await downloader.DownloadAsync(task.Url, stoppingToken);
						}
						catch (Exception ex)
						{
							_logger.LogError(ex, "Ошибка внутри Downloader для {Url}", task.Url);
							result = new DownloadResult(false, null, 0, ex.Message);
						}

						// 5. Сохранение результата и планирование следующего шага
						await scheduler.HandleDownloadResultAsync((ScrapingTask)task, result, stoppingToken);

						if (result.IsSuccess)
						{
							_logger.LogInformation("Scraping Worker: УСПЕХ {Url} (Status: {Code})", task.Url, result.StatusCode);
						}
						else
						{
							_logger.LogWarning("Scraping Worker: НЕУДАЧА {Url}. Ошибка: {Error}", task.Url, result.ErrorMessage);
						}
					}
				}
				catch (OperationCanceledException)
				{
					break;
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Scraping Worker: Критическая ошибка в главном цикле.");
					// Используем значение из конфигурации, хотя здесь лучше константа для безопасности
					await Task.Delay(_options.NoTasksDelayMs, stoppingToken);
				}
			}

			_logger.LogInformation("Scraping Worker: ОСТАНОВЛЕН.");
		}
	}
}