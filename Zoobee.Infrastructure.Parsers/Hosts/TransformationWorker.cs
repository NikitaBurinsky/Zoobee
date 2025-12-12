using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Transformation;

namespace Zoobee.Infrastructure.Parsers.Hosts
{
	public class TransformationWorker : BackgroundService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger<TransformationWorker> _logger;

		// Настройки частоты запуска (можно вынести в конфиг)
		private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(5);

		public TransformationWorker(
			IServiceProvider serviceProvider,
			ILogger<TransformationWorker> logger)
		{
			_serviceProvider = serviceProvider;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Transformation Worker started.");

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					// Создаем Scope, так как ITransformationService, DbContext и UnitOfWork - Scoped
					using (var scope = _serviceProvider.CreateScope())
					{
						var processingService = scope.ServiceProvider.GetRequiredService<ITransformationService>();

						// Запускаем цикл обработки
						await processingService.ProcessPendingDataAsync(stoppingToken);
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error in Transformation Worker loop.");
				}

				// Ждем перед следующим циклом (или используем PeriodicTimer в .NET 6+)
				await Task.Delay(_checkInterval, stoppingToken);
			}

			_logger.LogInformation("Transformation Worker stopped.");
		}
	}
}