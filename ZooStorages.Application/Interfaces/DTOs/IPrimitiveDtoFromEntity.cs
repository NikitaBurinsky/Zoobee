using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DtoTypes.Base;

namespace Zoobee.Application.Interfaces.DTOs
{
	public interface IPrimitiveDtoFromEntity<Dto, Entity>
		where Entity : class
	{ 
		public static abstract Dto FromEntity(Entity entity); 
	}	
}
