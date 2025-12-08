using Microsoft.EntityFrameworkCore;
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Entities.Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;
using static Zoobee.Domain.DataEntities.Base.IEntityMetadata;

namespace Zoobee.Infrastructure.Parsers.Services.Storage
{
	public class RawPageRepository : IRawPageRepository
	{
		private readonly IParsersDbContext _context;

		public RawPageRepository(IParsersDbContext context)
		{
			_context = context;
		}

		public async Task<List<RawPageEntity>> GetPendingPagesAsync(int batchSize, CancellationToken ct)
		{
			var now = DateTime.UtcNow;

			return await _context.RawPages
				.AsNoTracking() // Важно: AsNoTracking для чтения, чтобы не грузить ChangeTracker
				.Where(p => p.Status == RawPageStatus.Pending && p.NextTryAt <= now)
				.OrderBy(p => p.NextTryAt) // Приоритет тем, кто давно ждет
				.Take(batchSize)
				.ToListAsync(ct);
		}

		public async Task AddOrUpdateAsync(RawPageEntity page, CancellationToken ct)
		{
			// Если Id не установлен - это новая запись, иначе обновление
			if (page.Id == Guid.Empty)
			{
				await _context.RawPages.AddAsync(page, ct);
			}
			else
			{
				_context.RawPages.Update(page);
			}

			await _context.SaveChangesAsync(ct);
		}

		public async Task<bool> ExistsAsync(string url, CancellationToken ct)
		{
			return await _context.RawPages.AnyAsync(x => x.Url == url, ct);
		}

		public async Task BulkAddUrlsAsync(IEnumerable<string> urls, string sourceName, CancellationToken ct)
		{
			// 1. Находим те URL из списка, которых УЖЕ нет в базе.
			// При большом количестве ссылок это лучше делать батчами, 
			// но для старта подойдет простой подход.

			var distinctUrls = urls.Distinct().ToList();
			if (!distinctUrls.Any()) return;

			// Выбираем из базы те URL, которые совпадают с нашими
			var existingUrls = await _context.RawPages
				.Where(p => distinctUrls.Contains(p.Url))
				.Select(p => p.Url)
				.ToListAsync(ct);

			// Оставляем только те, которых нет в базе (HashSet для скорости поиска)
			var existingSet = new HashSet<string>(existingUrls);
			var newUrls = distinctUrls.Where(u => !existingSet.Contains(u)).ToList();

			if (!newUrls.Any()) return;

			//TODO Перенос логики Metadata в ParsersDbContext
			// Создаем сущности
			var newEntities = newUrls.Select(url => new RawPageEntity
			{
				Id = Guid.NewGuid(),
				Url = url,
				SourceName = sourceName,
				Status = RawPageStatus.Pending,
				NextTryAt = DateTime.UtcNow,
				Metadata = new EntityMetadata
				{
					 CreatedAt = DateTime.UtcNow,
					 LastModified = DateTime.UtcNow,
				}				
			});

			await _context.RawPages.AddRangeAsync(newEntities, ct);
			await _context.SaveChangesAsync(ct);
		}
	}
}