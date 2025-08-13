using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Domain.DataEntities.Products.Components.ExtendedAttributes;
using ZooStores.Application.DtoTypes.Base;

namespace ZooStorages.Domain.DataEntities.Products.Components.DynamicAttributes
{
	public class ProductAttributeTypeEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string AttributeName { get; set; }
		public ICollection<ProductAttributeEntity> AttributesOfType { get; set; }
	}

	public class ProductAttributeTypeEntityConfigurator : IEntityTypeConfiguration<ProductAttributeTypeEntity>
	{
		public void Configure(EntityTypeBuilder<ProductAttributeTypeEntity> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.AttributeName)
				.IsRequired(true);
			builder.HasMany(e => e.AttributesOfType)
				.WithOne(e => e.AttributeType)
				.OnDelete(DeleteBehavior.Cascade);
			builder.HasIndex(e => e.AttributeName).IsUnique(true);
		}
	}
}
