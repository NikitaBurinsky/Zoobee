namespace Zoobee.Infrastructure.Parsers.Core.Configuration
{
	public class ScrapingOptions
	{
		public const string SectionName = "Parsers:Scraping";

		// Задержки при ОШИБКАХ (1 мин, 5 мин...)
		public int[] RetryDelaysMinutes { get; set; } = Array.Empty<int>();

		// Частота УСПЕШНОГО скачивания (например, каждые 24 часа)
		public int DefaultParsingFrequencyHours { get; set; } = 24;
		
		//Scraping Worker
		public int RequestDelayMs { get; set; } = 1000;
		public int NoTasksDelayMs { get; set; } = 5000; // Задержка, если очередь пуста
	}
}
}