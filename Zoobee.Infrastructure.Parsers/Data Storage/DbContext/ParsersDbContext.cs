using Microsoft.EntityFrameworkCore;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain.DataEntities.Base.SoftDelete;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Entities.Failures.FailedSaveParsedItemTaskEntity;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;

namespace Zoobee.Infrastructure.Parsers.Data
{
	public class ParsersDbContext : DbContext, IParsersDbContext, IFailedTransformationsDbContext
	{
		public ParsersDbContext(DbContextOptions<ParsersDbContext> options) : base(options) { }

		public DbSet<ScrapingTask> ScrapingTasks { get; set; }
		public DbSet<ScrapingData> ScrapingDatas { get; set; }

		public DbSet<FailedToSaveParsedItemTaskEntity> FailedInfos { get; set; }
		public DbSet<BaseProductEntity> FailedProducts { get; set; }
		public DbSet<SellingSlotEntity> FailedSellings { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new FailedTransformationEntityConfigurator());

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseEntity).Assembly);
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseProductEntity).Assembly);

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
		public override int SaveChanges()
		{
			SetMetadata();
			return base.SaveChanges();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			SetMetadata();
			return await base.SaveChangesAsync(cancellationToken);
		}
		private void SetMetadata()
		{
			var entries = ChangeTracker.Entries()
				.Where(e => e.Entity is BaseEntity &&
						   (e.State == EntityState.Added || e.State == EntityState.Modified));

			var now = DateTimeOffset.UtcNow;

			foreach (var entry in entries)
			{
				var Entity = (BaseEntity)entry.Entity;

				if (entry.State == EntityState.Added)
					Entity.Metadata.CreatedAt = now;

				Entity.Metadata.LastModified = now;
			}

			// Обработка мягкого удаления
			var deletedEntries = ChangeTracker.Entries()
				.Where(e => e.Entity is ISoftDeletableEntity && e.State == EntityState.Deleted);

			foreach (var entry in deletedEntries)
			{
				entry.State = EntityState.Modified;
				var Entity = (ISoftDeletableEntity)entry.Entity;
				Entity.DeleteData.IsDeleted = true;
				Entity.DeleteData.DeletedAt = now;
			}
		}

	}
}