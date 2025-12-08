using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Services.Downloader
{
	public interface IHtmlDownloader
	{
		// Возвращает результат скачивания (контент, статус код, заголовки)
		// CancellationToken обязателен, чтобы корректно гасить потоки при остановке сервиса
		Task<DownloadResult> DownloadAsync(string url, CancellationToken ct);
	}

	public record DownloadResult(bool IsSuccess, string Content, int StatusCode, string ErrorMessage);
}
