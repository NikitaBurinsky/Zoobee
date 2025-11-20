using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Application.DtoTypes.Base;

namespace Zoobee.Domain.DataEntities.Environment.Manufactures
{
	public class SellerCompanyEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string CompanyName { get; set; }
		public string NormalizedCompanyName { get; set; }
	}

	public class SellerCompanyEntityConfigurator : IEntityTypeConfiguration<SellerCompanyEntity>
	{
		public void Configure(EntityTypeBuilder<SellerCompanyEntity> builder)
		{
			builder.Property(e => e.CompanyName)
				.HasMaxLength(60);
			builder.Property(e => e.NormalizedCompanyName)
				.HasMaxLength(60)
				.IsRequired(true);
			builder.HasIndex(e => e.NormalizedCompanyName)
				.IsUnique(true);
		}
	}


}
