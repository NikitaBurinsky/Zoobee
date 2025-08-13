using NetTopologySuite.Geometries;
using ZooStores.Application.DtoTypes.EntityComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZooStorages.Domain.DataEntities.Products;
using ZooStores.Application.DtoTypes.Base;

namespace ZooStores.Application.DtoTypes.Products.Delivery
{
	public class SelfPickupOptionEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public LocationEntity PickupPointLocation { get; set; }
		public uint? AvaibableInPlace { get; set; }
		public TimeSpan DeliveryToPointTime { get; set; }
		public decimal DeliveryToPointCost { get; set; }
		public ICollection<ProductSlotEntity> ProductSlots { get; set; }
		public ICollection<string> PaymentTypes { get; set; }
		//TODO Бронирование товара
		public bool IsAvaibableToBook { get; set; }
	}

	public class SelfPickupOptionEntityConfigurator : IEntityTypeConfiguration<SelfPickupOptionEntity>
	{
		public void Configure(EntityTypeBuilder<SelfPickupOptionEntity> builder)
		{
			builder.HasKey(e => e.Id);
			builder.HasOne(e => e.PickupPointLocation);
			builder.PrimitiveCollection(e => e.PaymentTypes);
			builder.HasMany(e => e.ProductSlots).WithMany(e => e.SelfPickupOptions);
		}
	}
}
