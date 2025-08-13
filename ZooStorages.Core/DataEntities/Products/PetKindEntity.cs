using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZooStores.Application.DtoTypes.Base;

namespace ZooStorages.Domain.DataEntities.Products
{
	public class PetKindEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string PetKindName { get; set; }
	}

	public class PetKindEntityConfigurator : IEntityTypeConfiguration<PetKindEntity>
	{
		public void Configure(EntityTypeBuilder<PetKindEntity> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.PetKindName).IsRequired(true);
			builder.HasIndex(e => e.PetKindName).IsUnique(true);
		}
	}
}
