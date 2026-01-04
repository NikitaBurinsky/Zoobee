using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain.DataEntities.Environment.Manufactures;
using Zoobee.Domain.DataEntities.SellingsInformation;

namespace Zoobee.Domain.DataEntities.Products
{
	public class SellingSlotEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string SellingUrl { get; set; }
		public float Rating { get; set; }
		public SellerCompanyEntity SellerCompany { get; set; }
		/// <summary>
		/// Базовая стоимость слота, без учета скидок
		/// </summary>
		public decimal DefaultSlotPrice { get; set; }
		public decimal Discount { get; set; }
		/// <summary>
		/// Итоговая стоимость, с учетом скидки 
		/// </summary>
		public decimal ResultPrice { get; set; }
		public BaseProductEntity Product { get; set; }
		public ICollection<DeliveryOptionEntity> DeliveryOptions { get; set; }
		public ICollection<SelfPickupOptionEntity> SelfPickupOptions { get; set; }
	}

	public class ProductSlotEntityConfigurator : IEntityTypeConfiguration<SellingSlotEntity>
	{
		public void Configure(EntityTypeBuilder<SellingSlotEntity> builder)
		{
			builder.HasOne(e => e.SellerCompany);
			builder.HasOne(e => e.Product)
				.WithMany(e => e.SellingSlots);
			builder.HasMany(e => e.DeliveryOptions)
				.WithMany(x => x.ProductSlots);
			builder.HasMany(e => e.SelfPickupOptions)
				.WithMany(x => x.ProductSlots);
		}
	}
}
