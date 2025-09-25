using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain.DataEntities.Environment.Manufactures;

namespace Zoobee.Domain.DataEntities.SellingsInformation
{
    public enum ProductAvaibablilityInStore
    {
        AvaibableInPlace = 0,
        /// <summary>
        /// Не находится в магазине на данный момент, но может быть туда доставлен и продан
        /// </summary>
        AvaibableWithDelivery,
        NotAvaibable,
    }

    public class SelfPickupOptionEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public ZooStoreEntity PickupPointLocation { get; set; }
        public TimeSpan DeliveryToPointTime { get; set; }
        public decimal DeliveryToPointCost { get; set; }
        public ICollection<SellingSlotEntity> ProductSlots { get; set; }
        public bool IsAvaibableToBook { get; set; }
        public bool IsAvaibableOnlinePayment { get; set; }
    }

    public class SelfPickupOptionEntityConfigurator : IEntityTypeConfiguration<SelfPickupOptionEntity>
    {
        public void Configure(EntityTypeBuilder<SelfPickupOptionEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.PickupPointLocation);
            builder.HasMany(e => e.ProductSlots)
				.WithMany(e => e.SelfPickupOptions);
			builder.HasMany(e => e.ProductSlots)
				.WithMany(e => e.SelfPickupOptions);
        }
    }
}
