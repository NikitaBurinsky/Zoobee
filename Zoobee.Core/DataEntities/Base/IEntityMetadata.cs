using Microsoft.EntityFrameworkCore;

namespace Zoobee.Domain.DataEntities.Base
{
	public interface IEntityMetadata
	{
		public EntityMetadata Metadata { get; set; }

		[Owned]
		public class EntityMetadata
		{
			public DateTimeOffset? CreatedAt { get; set; }
			public DateTimeOffset? LastModified { get; set; }
		}
	}


}
