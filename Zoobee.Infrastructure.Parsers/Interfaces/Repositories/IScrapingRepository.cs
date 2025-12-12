using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Enums; // Убедись, что ScrapingTaskType доступен

namespace Zoobee.Infrastructure.Parsers.Interfaces.Repositories
{
	public interface IScrapingRepository
	{
		// --- Блок Crawler (Скачивание) ---

		/// <summary>
		/// Получает список задач, которые нужно скачать (Status=Pending, NextTryAt <= Now).
		/// </summary>
		Task<List<ScrapingTask>> GetPendingTasksAsync(int batchSize, CancellationToken ct);

		/// <summary>
		/// Сохраняет результат скачивания и обновляет статус задачи.
		/// </summary>
		Task SaveExecutionResultAsync(ScrapingTask task, ScrapingData data, CancellationToken ct);

		/// <summary>
		/// Массовое добавление новых задач (Seeding). Игнорирует дубликаты.
		/// </summary>
		Task BulkAddTasksAsync(IEnumerable<(string Url, ScrapingTaskType Type)> tasks, string sourceName, CancellationToken ct);

		/// <summary>
		/// Проверяет наличие задачи (чтобы избежать дублей при ручном добавлении).
		/// </summary>
		Task<bool> TaskExistsAsync(string url, CancellationToken ct);


		// --- Блок Transformation (Обработка) ---

		/// <summary>
		/// Получает задачи, которые были успешно скачаны, но еще не трансформированы в товары.
		/// Возвращает кортеж: Сама задача (для метаданных) и Контент (HTML/XML).
		/// </summary>
		Task<List<(ScrapingTask Task, string Content)>> GetPendingTransformationTasksAsync(int batchSize, CancellationToken ct);

		/// <summary>
		/// Помечает задачу как "Трансформированную".
		/// Это может означать смену статуса задачи или установку флага IsProcessed в таблице данных.
		/// </summary>
		Task MarkAsTransformedAsync(Guid taskId, CancellationToken ct);
	}
}