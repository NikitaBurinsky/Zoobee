using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.UpdateProductsSpecificInfoProfiles
{
	public interface IUpdateProductSpecificProfile<Dto, Entity>
		where Dto : BaseProductDto
		where Entity : BaseProductEntity
	{
		public OperationResult UpdateSpecificInfo(Dto newInfo, Entity entityToUpdate);
	}
}
