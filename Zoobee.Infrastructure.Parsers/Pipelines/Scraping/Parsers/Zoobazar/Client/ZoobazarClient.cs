#define DEV
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.RegularExpressions;
using Zoobee.Domain;
using Zoobee.Infrastructure.Parsers.Storage.ParsedDataEntities.ParsedProductsModels.Zoobazar;
namespace Zoobee.Infrastructure.Parsers.Pipelines.Scraping.Parsers.Zoobazar.Client
	{
	public class ZoobazarClient
	{
		private readonly ILogger<ZoobazarClient> _logger;
		private readonly HttpClient _client;
		private readonly IConfiguration _configuration;

		public ZoobazarClient(ILogger<ZoobazarClient> logger, HttpClient client, IConfiguration configuration)
		{
			_logger = logger;
			_client = client;
			_configuration = configuration;
		}
		public static string ExtractProductCardJson(string html)
		{
			if (string.IsNullOrEmpty(html))
				return null;

			// Регулярное выражение для поиска JSON в атрибуте params
			string pattern = @"<product-card\s+params\s*=\s*'({.*?})'";

			Match match = Regex.Match(html, pattern, RegexOptions.Singleline);

			if (match.Success && match.Groups.Count > 1)
			{
				return match.Groups[1].Value;
			}

			return null;
		}
			
		public async Task<ZoobazarParsedProduct> ParseProductPageAsync(string url)
		{
			string json = "";
			try
			{
				_logger.LogInformation("Начинаем парсинг страницы: {Url}", url);

				// Получаем HTML содержимое страницы
				var html = await _client.GetStringAsync(url);
				json = ExtractProductCardJson(html);
				if (json == null)
				{
					_logger.LogWarning("Ошибка при парсинге страницы: {Url}. Не удалось извлечь json продукта с полученной страницы", url);
					return null;
				}

				var jsonOpt = new JsonSerializerOptions
				{
					AllowTrailingCommas = true,
					PropertyNameCaseInsensitive = true,
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				};
				var responseObj = JsonSerializer.Deserialize<ZoobazarParsedProduct>(json);

				return responseObj;
			}
			catch (JsonException ex)
			{
				//TODO Логирование в файл
				_logger.LogWarning(ex, "Ошибка при парсинге страницы: {Url}. Полученный json невозможно десериализовать. {Json}", url, json);
			}
			catch (HttpRequestException ex)
			{
				_logger.LogWarning(ex, "Ошибка при парсинге страницы: {Url}. Ошибка получения страницы.", url);
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Ошибка при парсинге страницы: {Url}. Неизвестная ошибка.", url);
			}
			return null;
		}

	}
}