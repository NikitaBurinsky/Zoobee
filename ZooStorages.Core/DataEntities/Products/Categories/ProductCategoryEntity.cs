using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Domain.DataEntities.Products;
using ZooStorages.Domain.DataEntities.Products.Components.ExtendedAttributes;
using ZooStores.Application.DtoTypes.Base;

namespace ZooStores.Application.DtoTypes.Products.Categories
{
    /// <summary>
    /// Категории представляют собой набор свойств, 
    /// </summary>
    public class ProductCategoryEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string CategoryName { get; set; }
		public string Description { get; set; }
		public ICollection<ProductTypeEntity> ProductTypes { get; set; }
	}


	public class ProductCategoryEntityConfigurator : IEntityTypeConfiguration<ProductCategoryEntity>
	{
		public void Configure(EntityTypeBuilder<ProductCategoryEntity> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.CategoryName).IsRequired(true);
			builder.Property(e => e.Description).IsRequired(true);
			builder.HasMany(o => o.ProductTypes).WithOne(m => m.Category);
			builder.HasIndex(e => e.CategoryName).IsUnique(true);
		}
	}
}
