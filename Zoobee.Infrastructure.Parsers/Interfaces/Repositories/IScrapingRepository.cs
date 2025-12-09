using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Enums;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Storage
{
	public interface IScrapingRepository
	{
		// Получить задачи для воркера
		Task<List<ScrapingTask>> GetPendingTasksAsync(int batchSize, CancellationToken ct);

		// Главный метод: Сохранить результат и обновить задачу (Транзакция)
		// Принимает новую сущность Data и обновленную сущность Task
		Task SaveExecutionResultAsync(ScrapingTask task, ScrapingData data, CancellationToken ct);

		// Массовое добавление новых URL (сидинг)
		public Task BulkAddTasksAsync(IEnumerable<(string Url, ScrapingTaskType Type)> tasks, string sourceName, CancellationToken ct);

		// Проверка существования
		Task<bool> TaskExistsAsync(string url, CancellationToken ct);
	}
}