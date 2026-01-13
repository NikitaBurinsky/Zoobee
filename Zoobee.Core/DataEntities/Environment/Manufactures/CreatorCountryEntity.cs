using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Application.DtoTypes.Base;

namespace Zoobee.Domain.DataEntities.Environment.Creators
{
	public class CreatorCountryEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string CountryNameEng { get; set; }
		public string CountryNameRus { get; set; }
		public string NormalizedCountryNameEng { get; set; }
		public string NormalizedCountryNameRus { get; set; }
	}

	public class CreatorCountryEntityConfigurator : IEntityTypeConfiguration<CreatorCountryEntity>
	{
		public void Configure(EntityTypeBuilder<CreatorCountryEntity> builder)
		{
			builder.Property(e => e.CountryNameEng)
				.IsRequired(true)
				.HasMaxLength(40);

			builder.Property(e => e.CountryNameRus)
				.IsRequired(true)
				.HasMaxLength(40);

		}
	}

}
