using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Application.DtoTypes.Base;

namespace Zoobee.Domain.DataEntities.Products
{
	public class PetKindEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string PetKindName { get; set; }
		public string NormalizedPetKindName { get; set; }
	}

	public class PetKindEntityConfigurator : IEntityTypeConfiguration<PetKindEntity>
	{
		public void Configure(EntityTypeBuilder<PetKindEntity> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.PetKindName)
				.HasMaxLength(30)
				.IsRequired(true);
			builder.Property(e => e.NormalizedPetKindName)
				.HasMaxLength(30)
				.IsRequired(true);
			builder.HasIndex(e => e.NormalizedPetKindName)
				.IsUnique(true);
		}
	}
}
