using Microsoft.EntityFrameworkCore;
using ZooStores.Application.DtoTypes.Clinics;
using ZooStores.Application.DtoTypes.Companies;
using ZooStores.Application.DtoTypes.Stores;
using ZooStores.Application.DtoTypes.Products.Categories;
using ZooStores.Application.DtoTypes.Products.Delivery;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStorages.Domain.DataEntities.Base.SoftDelete;
using ZooStorages.Domain.DataEntities.Products;
using ZooStorages.Domain.DataEntities.Products.Components.DynamicAttributes;
using ZooStorages.Domain.DataEntities.Products.Components.ExtendedAttributes;
using ZooStorages.Domain.DataEntities.Products.Components.Reviews;
using ZooStorages.Domain.DataEntities.Products.Components.Tags;
using ZooStores.Application.DtoTypes.Base;
using ZooStorages.Domain.DataEntities.Media;
using ZooStorages.Application.Interfaces.DbContext;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ZooStorages.Domain.DataEntities.Identity;

namespace ZooStores.Infrastructure.Repositoties
{
	public class ZooStoresDbContext : IdentityDbContext<TestUser>, IApplicationDbContext 
	{
		public DbSet<VetStoreEntity> VetStores { get; set; }
		public DbSet<VetClinicEntity> VetClinics { get; set; }
		public DbSet<CompanyEntity> Companies { get; set; }

		//Configured (для верхних конфигураторов еще нет) TODO

		public DbSet<ProductEntity> Products { get; set; }

		public DbSet<ProductTypeEntity> ProductTypes { get; set; }

		public DbSet<ProductCategoryEntity> ProductCategories { get; set; }

		public DbSet<PetKindEntity> PetKinds { get; set; }

		public DbSet<ProductSlotEntity> SellingSlots { get; set; }

		public DbSet<DeliveryOptionEntity> DeliveryOptions { get; set; }

		public DbSet<SelfPickupOptionEntity> SelfPickupOptions { get; set; }

		public DbSet<ProductAttributeTypeEntity> ExtAttributesTypes { get; set; }

		public DbSet<ProductAttributeEntity> ExtAttributes { get; set; }

		public DbSet<ReviewEntity> Reviews { get; set; }

		public DbSet<TagEntity> Tags { get; set; }
		public DbSet<MediaFileEntity> MediaFiles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			UseEntitiesConfigurators(modelBuilder);
		}

		protected void UseEntitiesConfigurators(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new TagEntityConfigurator());
			builder.ApplyConfiguration(new ReviewEntityConfigurator());
			builder.ApplyConfiguration(new ProductEntityConfigurator());
			builder.ApplyConfiguration(new ProductTypeEntityConfigurator());
			builder.ApplyConfiguration(new ProductCategoryEntityConfigurator());
			builder.ApplyConfiguration(new PetKindEntityConfigurator());
			builder.ApplyConfiguration(new ProductSlotEntityConfigurator());
			builder.ApplyConfiguration(new DeliveryOptionEntityConfigurator());
			builder.ApplyConfiguration(new SelfPickupOptionEntityConfigurator());
			builder.ApplyConfiguration(new ProductAttributeEntityConfigurator());
			builder.ApplyConfiguration(new ProductAttributeTypeEntityConfigurator());
			builder.ApplyConfiguration(new MediaFileEntityConfigurator());

			builder.ApplyConfigurationsFromAssembly(typeof(BaseEntity).Assembly);
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
				var entity = (BaseEntity)entry.Entity;

				if (entry.State == EntityState.Added)
				{
					entity.Metadata.CreatedAt = now;
				}

				entity.Metadata.LastModified = now;
			}

			// Обработка мягкого удаления
			var deletedEntries = ChangeTracker.Entries()
				.Where(e => e.Entity is ISoftDeletableEntity && e.State == EntityState.Deleted);

			foreach (var entry in deletedEntries)
			{
				entry.State = EntityState.Modified;
				var entity = (ISoftDeletableEntity)entry.Entity;
				entity.DeleteData.IsDeleted = true;
				entity.DeleteData.DeletedAt = now;
			}
		}

		public ZooStoresDbContext(DbContextOptions<ZooStoresDbContext> options) : base(options){
			Database.EnsureCreated();
		}
	}
}
