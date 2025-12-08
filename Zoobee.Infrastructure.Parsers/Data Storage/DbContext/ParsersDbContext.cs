using Microsoft.EntityFrameworkCore;
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Entities.Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;

namespace Zoobee.Infrastructure.Parsers.Data
{
	public class ParsersDbContext : DbContext, IParsersDbContext
	{
		public ParsersDbContext(DbContextOptions<ParsersDbContext> options) : base(options)
		{
		}

		public DbSet<RawPageEntity> RawPages { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Конфигурация сущности RawPage
			modelBuilder.Entity<RawPageEntity>(builder =>
			{
				builder.HasKey(x => x.Id);

				builder.HasIndex(x => x.Url)
					.IsUnique();

				// Составной индекс для очереди задач.
				// Самый частый запрос будет: "Дай мне Pending, у которых NextTryAt < Now"
				builder.HasIndex(x => new { x.Status, x.NextTryAt });

				// Настройки полей
				builder.Property(x => x.Url).IsRequired().HasMaxLength(2048); // Стандартная длина URL
				builder.Property(x => x.SourceName).HasMaxLength(100);

				// Content может быть NULL (если статус Pending или Failed)
				// Для Postgres используется text, для SQL Server - nvarchar(max)
				builder.Property(x => x.Content);
			});
		}
	}
}