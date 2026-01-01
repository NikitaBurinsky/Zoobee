using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Data;
using Zoobee.Infrastructure.Parsers.Interfaces.Repositories;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;

namespace Zoobee.Infrastructure.Parsers.Services.Storage
{
	public class ScrapingRepository : IScrapingRepository
	{
		private readonly IParsersDbContext _context;

		public ScrapingRepository(IParsersDbContext context)
		{
			_context = context;
		}

		// --- CRAWLER METHODS ---

		public async Task<List<ScrapingTask>> GetPendingTasksAsync(int batchSize, CancellationToken ct)
		{
			var now = DateTime.UtcNow;

			return await _context.ScrapingTasks
				.AsNoTracking()
				.Where(t => t.Status == RawPageStatus.Pending && t.NextTryAt <= now)
				.OrderBy(t => t.NextTryAt) // FIFO: Сначала самые старые долги
				.Take(batchSize)
				.ToListAsync(ct);
		}

		public async Task SaveExecutionResultAsync(ScrapingTask task, ScrapingData data, CancellationToken ct)
		{
			// 1. Добавляем снапшот в историю
			await _context.ScrapingDatas.AddAsync(data, ct);

			// 2. Обновляем статус задачи
			// Важно: Ставим Downloaded (1), чтобы TransformationWorker увидел эту задачу
			if (data.HttpStatusCode >= 200 && data.HttpStatusCode < 300)
			{
				task.Status = RawPageStatus.Downloaded;
				task.AttemptCount = 0; // Сбрасываем счетчик ошибок
			}
			else if (data.HttpStatusCode == 404)
			{
				task.Status = RawPageStatus.NotFound;
				// Можно увеличить NextTryAt надолго, если товар пропал
			}
			else
			{
				task.Status = RawPageStatus.Failed;
				task.AttemptCount++;
				// Тут можно добавить логику откладывания (Backoff) для Failed
			}

			task.Metadata.LastModified = DateTime.UtcNow;

			_context.ScrapingTasks.Update(task);
			await _context.SaveChangesAsync(ct);
		}

		public async Task BulkAddTasksAsync(IEnumerable<(string Url, ScrapingTaskType Type)> tasks, string sourceName, CancellationToken ct)
		{
			var tasksList = tasks.ToList();
			if (!tasksList.Any()) return;

			var distinctInputUrls = tasksList.Select(x => x.Url).Distinct().ToList();

			// 1. Ищем существующие (чтобы не дублировать)
			var existingUrls = await _context.ScrapingTasks
				.Where(t => distinctInputUrls.Contains(t.Url))
				.Select(t => t.Url)
				.ToListAsync(ct);

			// 2. Отбираем только новые
			var newItems = tasksList
				.Where(t => !existingUrls.Contains(t.Url))
				.GroupBy(x => x.Url) // Страховка от дублей во входном списке
				.Select(g => g.First())
				.ToList();

			if (!newItems.Any()) return;

			// 3. Создаем сущности
			var newEntities = newItems.Select(item => new ScrapingTask
			{
				Id = Guid.NewGuid(),
				Url = item.Url,
				SourceName = sourceName,
				Type = item.Type,
				Status = RawPageStatus.Pending,
				NextTryAt = DateTime.UtcNow, // Качаем сразу
				Metadata = new Domain.DataEntities.Base.IEntityMetadata.EntityMetadata
				{
					CreatedAt = DateTime.UtcNow,
					LastModified = DateTime.UtcNow
				}
			});

			await _context.ScrapingTasks.AddRangeAsync(newEntities, ct);
			await _context.SaveChangesAsync(ct);
		}

		public async Task<bool> TaskExistsAsync(string url, CancellationToken ct)
		{
			return await _context.ScrapingTasks.AnyAsync(t => t.Url == url, ct);
		}


		// --- TRANSFORMATION METHODS ---

		public async Task<List<(ScrapingTask Task, string Content)>> GetPendingTransformationTasksAsync(int batchSize, CancellationToken ct)
		{
			// Ищем задачи со статусом Downloaded (1)
			var result = await _context.ScrapingTasks
				.AsNoTracking()
				.Where(t => t.Status == RawPageStatus.Downloaded)
				.Select(t => new
				{
					Task = t,
					// Берем контент из последней успешной записи в истории
					LatestContent = t.History
						.OrderByDescending(h => h.Metadata.CreatedAt)
						.Select(h => h.Content)
						.FirstOrDefault()
				})
				.Take(batchSize)
				.ToListAsync(ct);

			// Фильтруем пустой контент и возвращаем кортежи
			return result
				.Where(x => !string.IsNullOrEmpty(x.LatestContent))
				.Select(x => (x.Task, x.LatestContent))
				.ToList();
		}

		public async Task MarkAsTransformedAsync(Guid taskId, CancellationToken ct)
		{
			var task = await _context.ScrapingTasks.FindAsync(new object[] { taskId }, ct);
			if (task == null) return;

			// Логика цикла:
			// 1. Трансформация прошла успешно.
			// 2. Переводим задачу обратно в Pending, чтобы она скачалась снова в будущем.
			// 3. (Опционально) Можно использовать статус Processed, если скачивание одноразовое.

			task.Status = RawPageStatus.Pending;

			// Планируем следующий запуск
			int frequency = task.CustomFrequencyHours ?? 24;
			task.NextTryAt = DateTime.UtcNow.AddHours(frequency);

			task.Metadata.LastModified = DateTime.UtcNow;

			_context.ScrapingTasks.Update(task);
			await _context.SaveChangesAsync(ct);
		}
	}
}