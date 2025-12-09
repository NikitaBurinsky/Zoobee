using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zoobee.Infrastructure.Parsers.Core.Configurations;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Seeding;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;

namespace Zoobee.Infrastructure.Parsers.Services.Seeding
{
	public class ScrapingSeeder : IScrapingSeeder
	{
		private ILogger<ScrapingSeeder> logger;
		private readonly IScrapingRepository _repository;
		private readonly ScrapingSeedingOptions _options;

		public ScrapingSeeder(IScrapingRepository repository, IOptions<ScrapingSeedingOptions> options)
		{
			_repository = repository;
			_options = options.Value;
		}

		public async Task SeedAsync(CancellationToken ct = default)
		{
			if (_options.Sources == null || !_options.Sources.Any())
				return;

			foreach (var source in _options.Sources)
			{
				var tasksToSeed = new List<(string Url, ScrapingTaskType Type)>();

				// Добавляем Sitemaps (с явным типом Sitemap)
				if (source.Sitemaps != null)
				{
					tasksToSeed.AddRange(source.Sitemaps.Select(url => (url, ScrapingTaskType.Sitemap)));
				}

				// Добавляем обычные стартовые ссылки (с типом ListingPage, т.к. обычно начинают с каталогов)
				// Или можно ставить Unknown, если не уверены
				if (source.StartUrls != null)
				{
					tasksToSeed.AddRange(source.StartUrls.Select(url => (url, ScrapingTaskType.ListingPage)));
				}

				if (tasksToSeed.Any())
				{
					await _repository.BulkAddTasksAsync(tasksToSeed, source.SourceName, ct);
				}
			}
		}
	}
}