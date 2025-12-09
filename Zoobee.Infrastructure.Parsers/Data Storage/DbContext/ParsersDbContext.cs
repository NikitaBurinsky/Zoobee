using Microsoft.EntityFrameworkCore;
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;

namespace Zoobee.Infrastructure.Parsers.Data
{
	public class ParsersDbContext : DbContext, IParsersDbContext
	{
		public ParsersDbContext(DbContextOptions<ParsersDbContext> options) : base(options) { }

		// Старой таблицы RawPages больше нет
		public DbSet<ScrapingTask> ScrapingTasks { get; set; }
		public DbSet<ScrapingData> ScrapingDatas { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// 1. Конфигурация Задачи (Task)
			modelBuilder.Entity<ScrapingTask>(b =>
			{
				b.HasKey(x => x.Id);
				b.Property(x => x.Url).IsRequired();
				b.Property(x => x.SourceName).IsRequired();

				// Уникальный URL (мы не хотим дублей задач)
				b.HasIndex(x => x.Url).IsUnique();

				// Индекс для очереди: ищем Pending задачи, время которых пришло
				b.HasIndex(x => new { x.Status, x.NextTryAt });
			});

			// 2. Конфигурация Истории (Data)
			modelBuilder.Entity<ScrapingData>(b =>
			{
				b.HasKey(x => x.Id);
				b.Property(x => x.Content).IsRequired(false); // Может быть пустым при ошибке

				// Связь "Один ко Многим"
				b.HasOne(x => x.ScrapingTask)
					.WithMany(x => x.History)
					.HasForeignKey(x => x.ScrapingTaskId)
					.OnDelete(DeleteBehavior.Cascade); // Удалил задачу -> удалил всю историю

				// Индекс для быстрой выборки истории по задаче
				b.HasIndex(x => x.ScrapingTaskId);
			});
		}
	}
}