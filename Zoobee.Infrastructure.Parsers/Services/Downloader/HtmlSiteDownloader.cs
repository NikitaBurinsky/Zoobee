using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Downloader;

namespace Zoobee.Infrastructure.Parsers.Services.Downloader
{
	// Регистрируется как Scoped
	public class HttpHtmlDownloader : IHtmlDownloader
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<HttpHtmlDownloader> _logger;

		// HttpClient должен быть предоставлен через DI (IHttpClientFactory)
		public HttpHtmlDownloader(HttpClient httpClient, ILogger<HttpHtmlDownloader> logger)
		{
			_httpClient = httpClient;
			_logger = logger;
			// Установка базовых заголовков для имитации браузера
			_httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
		}

		public async Task<DownloadResult> DownloadAsync(string url, CancellationToken ct)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				return new DownloadResult(false, null, 0, "URL не может быть пустым.");
			}

			try
			{
				using var request = new HttpRequestMessage(HttpMethod.Get, url);

				// Важно: HttpCompletionOption.ResponseHeadersRead, 
				// чтобы предотвратить считывание больших файлов в память до проверки статуса
				using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

				var statusCode = (int)response.StatusCode;

				if (response.IsSuccessStatusCode)
				{
					// Читаем контент только если код успешный (2xx)
					var content = await response.Content.ReadAsStringAsync(ct);
					return new DownloadResult(true, content, statusCode, null);
				}

				// Обработка ошибок, специфичных для сайтов (404 Not Found)
				if (statusCode == 404)
				{
					_logger.LogWarning("HTTP 404 Not Found: {Url}", url);
					return new DownloadResult(false, null, statusCode, "Not Found");
				}

				// Любая другая неудача (403, 500, 503)
				_logger.LogError("HTTP Error {StatusCode}: {Url}", statusCode, url);
				return new DownloadResult(false, null, statusCode, $"HTTP Error: {statusCode}");
			}
			catch (HttpRequestException httpEx)
			{
				// Ошибка сети или таймаут
				_logger.LogError(httpEx, "Network Error downloading {Url}", url);
				return new DownloadResult(false, null, 0, $"Network Error: {httpEx.Message}");
			}
			catch (OperationCanceledException)
			{
				// Отмена
				return new DownloadResult(false, null, 0, "Download cancelled");
			}
		}
	}
}