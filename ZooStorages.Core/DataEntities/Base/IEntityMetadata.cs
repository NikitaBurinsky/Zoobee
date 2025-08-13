using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooStorages.Domain.DataEntities.Base
{
	public interface IEntityMetadata
	{
		public EntityMetadata Metadata { get; set;  }

		[Owned]
		public class EntityMetadata
		{
			public DateTimeOffset? CreatedAt { get; set; }
			public DateTimeOffset? LastModified { get; set; }
		}
	}


}
