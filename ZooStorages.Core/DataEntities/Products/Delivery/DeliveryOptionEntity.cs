

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using ZooStorages.Domain.DataEntities.Products;
using ZooStores.Application.DtoTypes.Base;

namespace ZooStores.Application.DtoTypes.Products.Delivery
{
	/// <summary>
	/// Шаблон доставки
	/// </summary>
	public class DeliveryOptionEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public TimeSpan DeliveryTime { get; set; }
		public decimal DeliveryCost { get; set; }
		public Polygon? Area { get; set; }
		public List<string> PaymentTypes { get; set; }
		public ICollection<ProductSlotEntity> ProductSlots { get; set; }
	}

	public class DeliveryOptionEntityConfigurator : IEntityTypeConfiguration<DeliveryOptionEntity>
	{
		public void Configure(EntityTypeBuilder<DeliveryOptionEntity> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.DeliveryTime).IsRequired(true);
			builder.Property(e => e.DeliveryCost).IsRequired(true);
			builder.Property(e => e.Area).IsRequired(true);
			builder.HasMany(e => e.ProductSlots).WithMany(e => e.DeliveryOptions);
			builder.PrimitiveCollection(e => e.PaymentTypes);
		}
	}
}
