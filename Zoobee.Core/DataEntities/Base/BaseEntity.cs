using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Domain.DataEntities.Base;
using Zoobee.Domain.DataEntities.Base.SoftDelete;

namespace Zoobee.Application.DtoTypes.Base
{
	public class BaseEntity : ISoftDeletableEntity, IEntityMetadata
	{
		public ISoftDeletableEntity.SoftDeleteData DeleteData { get; set; }
		public IEntityMetadata.EntityMetadata Metadata { get; set; }
		public BaseEntity()
		{
			DeleteData = new ISoftDeletableEntity.SoftDeleteData();
			Metadata = new IEntityMetadata.EntityMetadata();
		}
	}

	public class BaseEntityConfigurator<T> : IEntityTypeConfiguration<T>
	where T : BaseEntity
	{
		public virtual void Configure(EntityTypeBuilder<T> builder)
		{
			builder.OwnsOne(x => x.DeleteData);
			builder.OwnsOne(x => x.Metadata);

			builder.HasQueryFilter(x => !x.DeleteData.IsDeleted);
		}
	}
}
