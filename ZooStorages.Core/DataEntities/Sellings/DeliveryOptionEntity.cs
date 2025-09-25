using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Domain.DataEntities.Environment.Geography;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Domain.DataEntities.SellingsInformation
{
	public enum DeliveryPointType
	{
		ToStore = 0,
		Post,
		Any,
	}

    /// <summary>
    /// Шаблон доставки
    /// </summary>
    public class DeliveryOptionEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public decimal DeliveryPrice { get; set; }
        public DeliveryAreaEntity DeliveryArea { get; set; }
        public ICollection<SellingSlotEntity> ProductSlots { get; set; }
		DeliveryPointType DeliveryType { get; set; }
		DateOnly DeliveryDays { get; set; } 
    }

    public class DeliveryOptionEntityConfigurator : IEntityTypeConfiguration<DeliveryOptionEntity>
    {
        public void Configure(EntityTypeBuilder<DeliveryOptionEntity> builder)
        {
            builder.HasKey(e => e.Id);
			builder.HasOne(x => x.DeliveryArea);
			builder.HasMany(x => x.ProductSlots).WithMany(e => e.DeliveryOptions);
        }
    }
}
