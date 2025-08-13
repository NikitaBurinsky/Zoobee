using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZooStorages.Domain.DataEntities.Products.Components.DynamicAttributes;
using ZooStores.Application.DtoTypes.Base;

namespace ZooStorages.Domain.DataEntities.Products.Components.ExtendedAttributes
{
	public class ProductAttributeEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public ProductAttributeTypeEntity AttributeType { get; set; }
		public ProductEntity? Product { get; set; }
		public string AttributeValue { get; set; } // "Красный", "L"
	}

	public class ProductAttributeEntityConfigurator : IEntityTypeConfiguration<ProductAttributeEntity>
	{
		public void Configure(EntityTypeBuilder<ProductAttributeEntity> builder)
		{
			builder.HasKey(e => e.Id);
			builder.HasOne(e => e.Product)
				.WithMany(e => e.ExtendedAttributes)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.ClientSetNull);
			builder.HasOne(e => e.AttributeType)
				.WithMany(e => e.AttributesOfType)
				.IsRequired(true); 
			builder.Property(e => e.AttributeValue)
				.IsRequired(true);
		}
	}
}
