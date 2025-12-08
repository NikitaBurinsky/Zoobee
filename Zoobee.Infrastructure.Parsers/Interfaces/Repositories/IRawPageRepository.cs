using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Entities.Zoobee.Infrastructure.Parsers.Core.Entities;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Storage
{
	public interface IRawPageRepository
	{
		/// <summary>
		/// Получает пакет задач на скачивание (у которых подошло время и статус Pending)
		/// </summary>
		Task<List<RawPageEntity>> GetPendingPagesAsync(int batchSize, CancellationToken ct);

		/// <summary>
		/// Добавляет или обновляет запись
		/// </summary>
		Task AddOrUpdateAsync(RawPageEntity page, CancellationToken ct);

		/// <summary>
		/// Массовое добавление новых ссылок (например, найденных в sitemap).
		/// Должен игнорировать дубликаты URL.
		/// </summary>
		Task BulkAddUrlsAsync(IEnumerable<string> urls, string sourceName, CancellationToken ct);

		/// <summary>
		/// Проверка, существует ли URL в базе
		/// </summary>
		Task<bool> ExistsAsync(string url, CancellationToken ct);
	}
}