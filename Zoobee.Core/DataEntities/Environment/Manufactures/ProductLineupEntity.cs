using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain.DataEntities.Environment.Creators;

namespace Zoobee.Domain.DataEntities.Environment.Manufactures
{
	public class ProductLineupEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public BrandEntity Brand { get; set; }
		public string LineupName { get; set; }
		public string NormalizedLineupName { get; set; }
		public string LineupDescription { get; set; }
	}



	public class ProductLineupEntityConfigurator : IEntityTypeConfiguration<ProductLineupEntity>
	{
		public void Configure(EntityTypeBuilder<ProductLineupEntity> builder)
		{
			builder.HasOne(x => x.Brand)
				.WithMany(x => x.BrandsLineups);
			builder.Property(x => x.LineupName)
				.HasMaxLength(60)
				.IsRequired(true);
			builder.Property(x => x.NormalizedLineupName)
				.HasMaxLength(60)
				.IsRequired(true);
			builder.Property(x => x.LineupDescription)
				.HasMaxLength(750);
		}
	}
}
