using Microsoft.EntityFrameworkCore;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain.DataEntities.Base.SoftDelete;
using Zoobee.Infrastructure.Parsers.Interfaces.Repositories.IParsersDbContext;
using Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsedDataEntities.ParsedDataIdentifiers;
using Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsedDataEntities.ParsedProductHolder;
using Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsedDataEntities.ParsedProductsModels.Zoobazar.EntityConfiguration;
using Zoobee.Infrastructure.Parsers.Storage.ParsedDataEntities.ParsedProductsModels.Zoobazar;

namespace Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsersDbContext
{
	public class ParsersDbContext : DbContext, IParsersDbContext
	{
		public DbSet<ParsedProductHolder<ZoobazarParsedProduct>> Zoobazar_ParsedProducts { get; set; }
		public DbSet<ParsedProductIdentifierEntity> ParsedProductsIdentifiers { get; set; }
		public DbSet<ZoobazarParsedProduct> ZoobazarParsedProducts { get; set; }
		public DbSet<Offer> ZoobazarOffers { get; set; }
		public DbSet<Tab> ZoobazarTabs { get; set; }

		protected void UseEntitiesConfigurators(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new ParsedProductHolderEntityConfigurator<ZoobazarParsedProduct>());
			builder.ApplyConfiguration(new ZoobazarParsedProductEntityConfigurator());
			builder.ApplyConfiguration(new ZoobazarTagEntityConfigurator());
			builder.ApplyConfiguration(new ZoobazarOfferEntityConfigurator());
		}

		public ParsersDbContext(DbContextOptions<ParsersDbContext> options) : base(options) 
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.EnableSensitiveDataLogging();
			optionsBuilder.EnableDetailedErrors();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			UseEntitiesConfigurators(modelBuilder);
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
				var Entity = (ParsedProductHolder)entry.Entity;

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

		public void ClearEntitiesTracking()
		{
			ChangeTracker.Clear();
		}
	}
}
