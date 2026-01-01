using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zoobee.Infrastructure.Parsers.Core.Configurations;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Interfaces.Repositories;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Seeding;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;

namespace Zoobee.Infrastructure.Parsers.Services.Seeding
{
	public class ScrapingSeeder : IScrapingSeeder
	{
		private ILogger<ScrapingSeeder> logger;
		private readonly IScrapingRepository _repository;
		private readonly ScrapingSeedingOptions _options;

		public ScrapingSeeder(IScrapingRepository repository, 
			ILogger<ScrapingSeeder> logger,
			IOptions<ScrapingSeedingOptions> options)
		{
			this.logger = logger;
			_repository = repository;
			_options = options.Value;
		}

		public async Task SeedAsync(CancellationToken ct = default)
		{
			if (_options.Sources == null || !_options.Sources.Any())
				return;

			List<string> SeededSitemaps = new List<string>();
			List<string> SeededUrls = new List<string>();
			foreach (var source in _options.Sources)
			{
				List<(string Url, ScrapingTaskType Type)> tasksToSeed = new List<(string Url, ScrapingTaskType Type)>();

				// Добавляем Sitemaps (с явным типом Sitemap)
				if (source.Sitemaps != null)
				{
					tasksToSeed.AddRange(source.Sitemaps.Select(url => (url, ScrapingTaskType.Sitemap)));
					SeededSitemaps.AddRange(tasksToSeed.Select(e => e.Url));
				}

				//Starts URLs its ProductPages (for test at least)
				if (source.StartUrls != null)
				{
					tasksToSeed.AddRange(source.StartUrls.Select(url => (url, ScrapingTaskType.ProductPage)));
					SeededUrls.AddRange(tasksToSeed.Select(e => e.Url));
				}

				if (tasksToSeed.Any())
				{
					await _repository.BulkAddTasksAsync(tasksToSeed, source.SourceName, ct);
				}
			}
			logger.LogInformation("URLS Seeded : @{SeededUrls}", SeededUrls);
			logger.LogInformation("Sitemaps Seeded : @{SeededSitemaps}", SeededSitemaps);
		}
	}
}