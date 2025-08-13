using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using ZooStorages.Domain.DataEntities.Products.Components.Dimensions;
using ZooStorages.Domain.DataEntities.Products.Components.Reviews;
using ZooStorages.Domain.DataEntities.Products.Components.Tags;
using ZooStorages.Domain.Enums;
using ZooStores.Application.DtoTypes.Base;
using ZooStores.Application.DtoTypes.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZooStorages.Domain.DataEntities.Products.Components.Attributes;
using ZooStorages.Domain.DataEntities.Products.Components.ExtendedAttributes;

namespace ZooStorages.Domain.DataEntities.Products.Components
{
    public class ProductEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
		public string NormalizedName { get; set; }
        public string Information { get; set; }
        public ProductTypeEntity ProductType { get; set; }
        //PRODUCT ATTRIBUTES
        public ManufactureAttributes ManufactureAttributes { get; set; }//owned
        public PhysicalAttributes PhysicalAttributes { get; set; }//owned
		public PetProductAttributes PetInfoAttributes { get; set; }//owned
		public List<string> MediaURI { get; set; } = new List<string>();
		public ICollection<ProductSlotEntity> SellingSlots { get; set; }
        public ICollection<ReviewEntity> Reviews { get; set; }
        public ICollection<ProductAttributeEntity> ExtendedAttributes { get; set; }
        public ICollection<TagEntity> Tags;
    }

    public class ProductEntityConfigurator : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired(true);
            builder.Property(e => e.Information).IsRequired(true);
			builder.HasOne(e => e.ProductType).WithMany(e => e.ProductsOfType);
			builder.OwnsOne(e => e.ManufactureAttributes, ma =>
			{
				ma.Property(e => e.Brand).IsRequired(true);
				ma.Property(e => e.UPC_Code).HasMaxLength(12);
				ma.Property(e => e.EAN_Code).HasMaxLength(13);
			});
			builder.OwnsOne(e => e.PetInfoAttributes, pa =>
			{
				pa.HasOne(e => e.PetKind);
			});
			builder.OwnsOne(e => e.PhysicalAttributes, pa =>
			{
				pa.OwnsOne(e => e.Dimensions);
			});
			builder.PrimitiveCollection(e => e.MediaURI);
			builder.HasMany(e => e.SellingSlots).WithOne(e => e.Product);
			builder.HasMany(e => e.Reviews);
			builder.HasMany(e => e.Tags);
			builder.HasIndex(e => e.Name).IsUnique();
		}
    }


}
