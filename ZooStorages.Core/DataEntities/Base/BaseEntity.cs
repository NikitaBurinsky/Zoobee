using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Domain.DataEntities.Base;
using ZooStorages.Domain.DataEntities.Base.SoftDelete;

namespace ZooStores.Application.DtoTypes.Base
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
