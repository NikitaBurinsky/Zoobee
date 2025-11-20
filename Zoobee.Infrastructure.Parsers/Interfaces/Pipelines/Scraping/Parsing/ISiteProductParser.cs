namespace Zoobee.Infrastructure.Parsers.Interfaces.Pipelines.Scraping.Parsing
{
	public interface ISiteProductParser<ParsedProductType>
		where ParsedProductType : class
	{
		public Task<(ParsedProductType?, string? parsedUrl)> ParseAndGetNextAsync();
		/// <summary>
		/// Переводит sitemap.xml в список парсируемых страниц. Принимает строку локального пути к файлу, либо url в сети
		/// </summary>
		public Task<List<string>> GetSitemapUrlsAsync(List<string> sitemapsPathsOrUrls);
		/// <summary>
		/// Возвращает url который будет распаршен при следующей итерации ParseAndGetNextAsync()
		/// </summary>
		/// <returns></returns>
		public string GetNextParsableUrl();
		/// <summary>
		/// Пропустить следующий url.
		/// Если был пропущен последний url в списке (начался новый круг), вернет true
		/// </summary>
		public bool SkipUrl();
		/// <summary>
		/// Список Url что данный парсер будет парсить
		/// </summary>
		public IList<string> ParsableUrls { get; }
	}
}
