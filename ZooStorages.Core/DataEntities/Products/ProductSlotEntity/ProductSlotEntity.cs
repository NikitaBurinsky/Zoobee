using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStores.Application.DtoTypes.Products.Delivery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZooStores.Application.DtoTypes.Base;

namespace ZooStorages.Domain.DataEntities.Products
{
    public class ProductSlotEntity : BaseEntity
    {
        public Guid Id { get; set; }
		public ProductEntity Product { get; set; }
        public decimal DefaultSlotPrice { get; set; }
        public decimal Discount { get; set; }
		public bool IsAvaibable { get; set; }
        public ICollection<DeliveryOptionEntity> DeliveryOptions { get; set; }
        public ICollection<SelfPickupOptionEntity> SelfPickupOptions { get; set; }
    }

	public class ProductSlotEntityConfigurator : IEntityTypeConfiguration<ProductSlotEntity>
	{
		public void Configure(EntityTypeBuilder<ProductSlotEntity> builder)
		{
			builder.HasKey(e => e.Id);
			builder.HasOne(e => e.Product).WithMany(e => e.SellingSlots).IsRequired(true);
			builder.Property(e => e.IsAvaibable).IsRequired(true);
			builder.HasMany(e => e.DeliveryOptions).WithMany(e => e.ProductSlots);
			builder.HasMany(e => e.SelfPickupOptions).WithMany(e => e.ProductSlots);
		}
	}




}
