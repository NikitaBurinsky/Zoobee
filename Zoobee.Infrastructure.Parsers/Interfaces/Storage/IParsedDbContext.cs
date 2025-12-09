using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Core.Entities;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Storage
{
	public interface IParsersDbContext
	{
		public DbSet<ScrapingData> ScrapingDatas { get; set; }
		public DbSet<ScrapingTask> ScrapingTasks { get; set; }

		// Метод сохранения изменений
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

		// Доступ к инфраструктуре БД (нужен, например, для миграций или транзакций)
		Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }
	}
}
