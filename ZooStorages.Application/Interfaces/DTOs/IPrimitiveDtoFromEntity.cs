using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStores.Application.DtoTypes.Base;

namespace ZooStorages.Application.Interfaces.DTOs
{
	public interface IPrimitiveDtoFromEntity<Dto, Entity>
		where Entity : class
	{ 
		public static abstract Dto FromEntity(Entity entity); 
	}
}
