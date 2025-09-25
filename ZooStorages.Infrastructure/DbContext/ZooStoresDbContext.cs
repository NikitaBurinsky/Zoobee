using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Zoobee.Domain.DataEntities.Base.SoftDelete;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain.DataEntities.Media;
using Zoobee.Domain.DataEntities.Catalog.Reviews;
using Zoobee.Domain.DataEntities.SellingsInformation;
using Zoobee.Domain.DataEntities.Catalog.Tags;
using Zoobee.Domain.DataEntities.Environment.Creators;
using Zoobee.Domain.DataEntities.Environment.Manufactures;
using Zoobee.Domain.DataEntities.Environment.Geography;
using Zoobee.Application.Interfaces.DbContext;
using Zoobee.Domain.DataEntities.Identity.Users;
using Zoobee.Domain.DataEntities.Identity.Role;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Zoobee.Infrastructure.Repositoties
{
    public class ZooStoresDbContext : IdentityDbContext<BaseApplicationUser, ApplicationRole, Guid>, IApplicationDbContext
	{

		//Configured (для верхних конфигураторов еще нет) TODO
		public DbSet<BaseProductEntity> Products { get; set; }
		public DbSet<SellingSlotEntity> SellingSlots { get; set; }
		public DbSet<DeliveryOptionEntity> DeliveryOptions { get; set; }
		public DbSet<SelfPickupOptionEntity> SelfPickupOptions { get; set; }
		public DbSet<ReviewEntity> Reviews { get; set; }
		public DbSet<TagEntity> Tags { get; set; }
		public DbSet<LocationEntity> Locations { get; set; }
		public DbSet<PetKindEntity> PetKinds { get; set; }
		public DbSet<BrandEntity> Brands { get; set; }
		public DbSet<CreatorCountryEntity> CreatorCountries { get; set; }
		public DbSet<CreatorCompanyEntity> CreatorCompanies { get; set; }
		public DbSet<ProductLineupEntity> ProductsLineups { get; set; }
		public DbSet<SellerCompanyEntity> SellerCompanies { get; set; }
		public DbSet<ZooStoreEntity> ZooStores { get; set; }
		public DbSet<DeliveryAreaEntity> DeliveryAreas { get; set; }
		public DbSet<MediaFileEntity> MediaFiles { get; set; }//+
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			UseEntitiesConfigurators(modelBuilder);
		}

		protected void UseEntitiesConfigurators(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new TagEntityConfigurator());
			builder.ApplyConfiguration(new ReviewEntityConfigurator());
			builder.ApplyConfiguration(new BaseProductEntityConfigurator());
			builder.ApplyConfiguration(new PetKindEntityConfigurator());
			builder.ApplyConfiguration(new ProductSlotEntityConfigurator());
			builder.ApplyConfiguration(new DeliveryOptionEntityConfigurator());
			builder.ApplyConfiguration(new SelfPickupOptionEntityConfigurator());
			builder.ApplyConfiguration(new MediaFileEntityConfigurator());

			builder.ApplyConfiguration(new AdminUserEntityConfigurator());
			builder.ApplyConfiguration(new OrganisationUserEntityConfigurator());
			builder.ApplyConfiguration(new CustomerUserEntityConfigurator());

			builder.ApplyConfigurationsFromAssembly(typeof(BaseEntity).Assembly);
			builder.ApplyConfigurationsFromAssembly(typeof(BaseApplicationUser).Assembly);
			builder.ApplyConfigurationsFromAssembly(typeof(BaseProductEntity).Assembly);



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

		public ZooStoresDbContext(DbContextOptions<ZooStoresDbContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
	}
}
