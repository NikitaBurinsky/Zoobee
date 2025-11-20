using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain.DataEntities.Environment.Manufactures;

namespace Zoobee.Domain.DataEntities.Environment.Creators
{
	public class BrandEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string BrandName { get; set; }
		public string NormalizedBrandName { get; set; }
		public string Description { get; set; }
		public ICollection<ProductLineupEntity> BrandsLineups { get; set; }
	}

	public class BrandEntityConfigurator : IEntityTypeConfiguration<BrandEntity>
	{
		public void Configure(EntityTypeBuilder<BrandEntity> builder)
		{
			builder.HasIndex(e => e.NormalizedBrandName)
				.IsUnique();
			builder.Property(e => e.NormalizedBrandName)
				.IsRequired(true)
				.HasMaxLength(60);
			builder.Property(e => e.BrandName)
				.HasMaxLength(60)
				.IsRequired(true);
			builder.Property(e => e.Description)
				.HasMaxLength(750);
			builder.HasMany(e => e.BrandsLineups)
				.WithOne(e => e.Brand);
		}
	}
}
