using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Application.DtoTypes.Base;

namespace Zoobee.Domain.DataEntities.Environment.Manufactures
{
	public class CreatorCompanyEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string CompanyName { get; set; }
		public string NormalizedCompanyName { get; set; }
	}

	public class CreatorCompanyEntityConfigurator : IEntityTypeConfiguration<CreatorCompanyEntity>
	{
		public void Configure(EntityTypeBuilder<CreatorCompanyEntity> builder)
		{
			builder.HasIndex(c => c.NormalizedCompanyName)
				.IsUnique(true);
			builder.Property(x => x.CompanyName)
				.HasMaxLength(60)
				.IsRequired(true);
			builder.Property(x => x.NormalizedCompanyName)
				.HasMaxLength(60)
				.IsRequired(true);
		}
	}
}
