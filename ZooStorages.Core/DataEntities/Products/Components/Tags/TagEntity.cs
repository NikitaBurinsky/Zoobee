using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZooStores.Application.DtoTypes.Base;

namespace ZooStorages.Domain.DataEntities.Products.Components.Tags
{
	public class TagEntity : BaseEntity
    {
		public Guid Id { get; set; }
        public string TagName { get; set; }
    }

	public class TagEntityConfigurator : IEntityTypeConfiguration<TagEntity>
	{
		public void Configure(EntityTypeBuilder<TagEntity> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.TagName)
				.HasMaxLength(40)
				.IsRequired(true);
			builder.HasIndex(e => e.TagName).IsUnique(true);
		}
	}

}
