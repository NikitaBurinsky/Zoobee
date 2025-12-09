using Microsoft.EntityFrameworkCore;
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;
using Zoobee.Infrastructure.Parsers.Data; // Для ParsersDbContext

namespace Zoobee.Infrastructure.Parsers.Services.Storage
{
	public class ScrapingRepository : IScrapingRepository
	{
		private readonly IParsersDbContext _context;

		public ScrapingRepository(IParsersDbContext context)
		{
			_context = context;
		}

		public async Task<List<ScrapingTask>> GetPendingTasksAsync(int batchSize, CancellationToken ct)
		{
			var now = DateTime.UtcNow;

			return await _context.ScrapingTasks
				.AsNoTracking() // Читаем без трекинга для скорости
				.Where(t => t.Status == RawPageStatus.Pending && t.NextTryAt <= now)
				.OrderBy(t => t.NextTryAt) // Сначала самые старые долги
				.Take(batchSize)
				.ToListAsync(ct);
		}

		public async Task SaveExecutionResultAsync(ScrapingTask task, ScrapingData data, CancellationToken ct)
		{
			// Мы выполняем это в одной транзакции (EF Core делает это автоматически при SaveChanges)

			// 1. Добавляем запись в историю
			await _context.ScrapingDatas.AddAsync(data, ct);

			// 2. Обновляем статус задачи
			// Так как мы читали Task через AsNoTracking, нам нужно явно приаттачить его
			_context.ScrapingTasks.Update(task);

			await _context.SaveChangesAsync(ct);
		}

		public async Task BulkAddTasksAsync(IEnumerable<(string Url, ScrapingTaskType Type)> tasks, string sourceName, CancellationToken ct)
		{
			var tasksList = tasks.ToList();
			if (!tasksList.Any()) return;

			var distinctInputUrls = tasksList.Select(x => x.Url).Distinct().ToList();

			// 1. Находим те URL, которые УЖЕ есть в базе
			var existingUrls = await _context.ScrapingTasks
				.Where(t => distinctInputUrls.Contains(t.Url))
				.Select(t => t.Url)
				.ToListAsync(ct);

			// 2. Отбираем только новые
			var newItems = tasksList
				.Where(t => !existingUrls.Contains(t.Url))
				.GroupBy(x => x.Url) // На случай дублей внутри самого списка tasksList
				.Select(g => g.First())
				.ToList();

			if (!newItems.Any()) return;

			// 3. Создаем сущности
			var newEntities = newItems.Select(item => new ScrapingTask
			{
				Id = Guid.NewGuid(),
				Url = item.Url,
				SourceName = sourceName,
				Type = item.Type, // <-- Записываем тип
				Status = RawPageStatus.Pending,
				NextTryAt = DateTime.UtcNow,
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
	}
}