using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStores.Application.DtoTypes.Products.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZooStores.Application.DtoTypes.Base;

namespace ZooStorages.Domain.DataEntities.Products
{
	public class ProductTypeEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Information { get; set; }
		public ProductCategoryEntity Category { get; set; }
		//Items of type
		public ICollection<ProductEntity> ProductsOfType { get; set; }
	}

	public class ProductTypeEntityConfigurator : IEntityTypeConfiguration<ProductTypeEntity>
	{
		public void Configure(EntityTypeBuilder<ProductTypeEntity> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.Name).IsRequired(true);
			builder.HasOne(e => e.Category).WithMany(e => e.ProductTypes).IsRequired(true);
			builder.HasMany(e => e.ProductsOfType).WithOne(e => e.ProductType);
			builder.HasIndex(e => e.Name).IsUnique(true);
		}
	}
}


