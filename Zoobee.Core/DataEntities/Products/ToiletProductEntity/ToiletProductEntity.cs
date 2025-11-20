using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Domain.DataEntities.Data_Primitives;

namespace Zoobee.Domain.DataEntities.Products.ToiletProductEntity
{
	public enum ToiletType
	{
		Open = 0,
		Close,
		BioToilet,
		Automatic,
		Diapers,
		None
	}
	public class ToiletProductEntity : BaseProductEntity
	{
		public ToiletType ToiletType { get; set; }
		public SizeDimensions? Dimensions { get; set; }
		public float? VolumeLiters { get; set; }
		public PetAgeRange? PetAgeRange { get; set; }
	}

	public class ToiletProductEntityConfigurator : IEntityTypeConfiguration<ToiletProductEntity>
	{
		public void Configure(EntityTypeBuilder<ToiletProductEntity> builder)
		{
			builder.OwnsOne(e => e.Dimensions);
			builder.OwnsOne(e => e.PetAgeRange);
			builder.Property(e => e.ToiletType)
				.IsRequired(true);
		}
	}
}
