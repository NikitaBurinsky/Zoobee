using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Zoobee.Infrastructure.Parsers.Interfaces.Pipelines.Scraping.ParsingQueue;

namespace Zoobee.Infrastructure.Parsers.Host
{
	public class ZoobeeParsersWorker : BackgroundService
	{
		private ILogger<ZoobeeParsersWorker> logger;
		private IServiceProvider serviceProvider;
		public ZoobeeParsersWorker(ILogger<ZoobeeParsersWorker> logger,
			IServiceProvider services)
		{
			this.logger = logger;
			this.serviceProvider = services;
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			Task parseTask = ExecuteParsingQueue(stoppingToken);
			Task transformTask = ExecuteParseDataBaseTransform(stoppingToken);
			return Task.WhenAll(parseTask, transformTask);
		}

		protected async Task ExecuteParsingQueue(CancellationToken cancellationToken)
		{
			IServiceScope scope = serviceProvider.CreateScope();
			var parsingQueue = scope.ServiceProvider.GetRequiredService<IParsingQueueService>();
			logger.LogInformation("Parsing queue executed");
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					await parsingQueue.HandleNext();
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "Parsing caused exception");
				}
			}
		}

		protected async Task ExecuteParseDataBaseTransform(CancellationToken cancellationToken)
		{
			return;
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
				}
				catch
				{

				}
			}
			return;
		}







		public override Task StartAsync(CancellationToken cancellationToken)
		{
			return base.StartAsync(cancellationToken);
		}
		public override Task StopAsync(CancellationToken cancellationToken)
		{
			return base.StopAsync(cancellationToken);
		}


	}
}
