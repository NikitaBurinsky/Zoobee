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
				.OrderBy(t => t.NextTryAt) // FIFO: Сначала старые
				.Take(batchSize)
				.ToListAsync(ct);
		}

		public async Task SaveExecutionResultAsync(ScrapingTask task, ScrapingData data, CancellationToken ct)
		{
			// 1. Добавляем снапшот в историю
			await _context.ScrapingDatas.AddAsync(data, ct);

			// 2. Обновляем статус задачи
			// Важно: Мы ставим статус Success, что является сигналом для TransformationWorker
			task.Status = RawPageStatus.Downloaded;
			task.AttemptCount = 0; // Сбрасываем счетчик ошибок при успехе
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

			// 3. Маппим в сущности
			var newEntities = newItems.Select(item => new ScrapingTask
			{
				Id = Guid.NewGuid(),
				Url = item.Url,
				SourceName = sourceName,
				Type = item.Type, // <-- Сохраняем тип (Sitemap/Product)
				Status = RawPageStatus.Pending,
				NextTryAt = DateTime.UtcNow, // Новые задачи качаем сразу
				Metadata = new Domain.DataEntities.Base.IEntityMetadata.EntityMetadata
				{
					CreatedAt = DateTime.UtcNow,
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
			// Нам нужны задачи, которые успешно скачались (Success), но еще не обработаны.
			// Мы берем HTML контент из последней записи в истории (History).

			var result = await _context.ScrapingTasks
				.AsNoTracking()
				.Where(t => t.Status == RawPageStatus.Success)
				.Select(t => new
				{
					Task = t,
					// Берем контент самой свежей записи ScrapingData для этой задачи
					LatestContent = t.History
						.OrderByDescending(h => h.CreatedAt) // Assuming CreatedAt is available via BaseEntity inheritance or Metadata
						.Select(h => h.Content)
						.FirstOrDefault()
				})
				.Take(batchSize)
				.ToListAsync(ct);

			// Возвращаем кортежи, фильтруя пустой контент
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
			// 2. Переводим задачу обратно в Pending.
			// 3. Планируем следующий запуск (NextTryAt) через N часов.

			task.Status = RawPageStatus.Pending;

			// Если частота не задана индивидуально, берем дефолт (например, 24 часа)
			int frequency = task.CustomFrequencyHours ?? 24;
			task.NextTryAt = DateTime.UtcNow.AddHours(frequency);

			task.Metadata.UpdatedAt = DateTime.UtcNow;

			_context.ScrapingTasks.Update(task);
			await _context.SaveChangesAsync(ct);
		}
	}
}