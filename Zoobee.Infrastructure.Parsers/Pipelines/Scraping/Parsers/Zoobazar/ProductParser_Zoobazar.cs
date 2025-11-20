using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Zoobee.Infrastructure.Parsers.Interfaces.Pipelines.Scraping.Parsing;
using Zoobee.Infrastructure.Parsers.Pipelines.Scraping.Parsers.Zoobazar.Client;
using Zoobee.Infrastructure.Parsers.Storage.ParsedDataEntities.ParsedProductsModels.Zoobazar;

namespace Zoobee.Infrastructure.Parsers.Pipelines.Scraping.Parsers.Zoobazar
{
	public class ProductParser_Zoobazar : ISiteProductParser<ZoobazarParsedProduct>
	{
		private List<string> SitemapFiles { get; set; }

		public IList<string> ParsableUrls => Urls;

		private List<string> Urls;
		private IConfiguration configuration;
		private IHttpClientFactory httpClientFactory;
		private ILogger<ProductParser_Zoobazar> logger;
		private ZoobazarClient ZoobazarPageParser;
		private int currentPageIndex = -1;

		public ProductParser_Zoobazar(IConfiguration configuration,
			IHttpClientFactory httpClientFactory,
			ZoobazarClient zoobazarPageParser,
			ILogger<ProductParser_Zoobazar> logger)
		{
			SitemapFiles = configuration.GetSection("Parsing:Sitemaps:Zoobazar")
				.Get<List<string>>() ?? new List<string>();
			this.httpClientFactory = httpClientFactory;
			this.configuration = configuration;
			ZoobazarPageParser = zoobazarPageParser;
			this.logger = logger;
			Urls = GetSitemapUrlsAsync(SitemapFiles).Result;
			if (Urls.Count == 0)
			{
				throw new ArgumentException($"- Urls in sitemap not found in {GetType().Name}");
			}
		}

		public async Task<(ZoobazarParsedProduct?, string? parsedUrl)> ParseAndGetNextAsync()
		{
			++currentPageIndex;
			if (currentPageIndex >= Urls.Count)
			{
				currentPageIndex = 0;
			}
			var url = Urls[currentPageIndex];
			var result = await ZoobazarPageParser.ParseProductPageAsync(url);
			return result != null ?
				(result, url) : (null,null);
		}

		public async Task<List<string>> GetSitemapUrlsAsync(List<string> pathOrUrls)
		{
			List<string> result = new List<string>();
			foreach (var pathOrUrl in pathOrUrls)
			{
				string content;
				if (IsWebUrl(pathOrUrl))
				{ //By url
					var httpClient = httpClientFactory.CreateClient();
					content = await httpClient.GetStringAsync(pathOrUrl);
				}
				else
				{ //Local file
					if (!File.Exists(pathOrUrl))
					{
						throw new FileNotFoundException($"- Sitemap file for {GetType().Name} not found");
					}
					content = await File.ReadAllTextAsync(pathOrUrl);
				}
				result.AddRange(ParseSitemapUrls(content));
			}
			return result;
		}

		private static bool IsWebUrl(string input)
		{
			return input.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
				|| input.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
		}
		private static List<string> ParseSitemapUrls(string xmlContent)
		{
			var urls = new List<string>();
			using (var reader = System.Xml.XmlReader.Create(new StringReader(xmlContent)))
			{
				while (reader.Read())
				{
					if (reader.NodeType == System.Xml.XmlNodeType.Element && reader.Name == "loc")
					{
						var url = reader.ReadElementContentAsString();
						urls.Add(url);
					}
				}
			}
			return urls;
		}

		public string GetNextParsableUrl()
		{
			int curNext = currentPageIndex + 1;
			return curNext < Urls.Count ?
				Urls[curNext] :
				Urls.FirstOrDefault();
		}

		public bool SkipUrl()
		{
			++currentPageIndex;
			if (currentPageIndex >= Urls.Count)
			{
				currentPageIndex = 0;
				return true;
			}
			return false;
		}
	}
}
