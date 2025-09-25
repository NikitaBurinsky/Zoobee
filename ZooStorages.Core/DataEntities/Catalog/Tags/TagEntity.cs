using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using Zoobee.Application.DtoTypes.Base;

namespace Zoobee.Domain.DataEntities.Catalog.Tags
{
	public class TagEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string TagName { get; set; }
		public string NormalizedTagName { get; set; }
		public Type ProductType { get; set; }

	}

	public class TagEntityConfigurator : IEntityTypeConfiguration<TagEntity>
	{
		public void Configure(EntityTypeBuilder<TagEntity> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.TagName)
				.HasMaxLength(40)
				.IsRequired(true);
			builder.Property(e => e.NormalizedTagName)
				.IsRequired(true)
				.HasMaxLength(40);
			builder.HasIndex(e => e.NormalizedTagName)
				.IsUnique(true);

			builder.Property(e => e.ProductType)
				.HasConversion<string?>(
					t => t.AssemblyQualifiedName,
					t => Type.GetType(t)
				);
		}
	}

}
