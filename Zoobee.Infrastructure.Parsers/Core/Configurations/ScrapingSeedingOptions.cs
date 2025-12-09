using System.Collections.Generic;

namespace Zoobee.Infrastructure.Parsers.Core.Configurations
{
	public class ScrapingSeedingOptions
	{
		public List<ScrapingSourceConfig> Sources { get; set; } = new();
	}

	public class ScrapingSourceConfig
	{
		public string SourceName { get; set; } // "Zoobazar"
		public List<string> Sitemaps { get; set; } = new(); // Список ссылок на sitemaps
		public List<string> StartUrls { get; set; } = new(); // Просто стартовые ссылки (например, корни каталогов)
	}
}