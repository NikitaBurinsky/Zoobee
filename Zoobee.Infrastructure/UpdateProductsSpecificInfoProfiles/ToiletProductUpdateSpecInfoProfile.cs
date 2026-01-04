using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.UpdateProductsSpecificInfoProfiles
{
	public class ToiletProductUpdateSpecInfoProfile : IUpdateProductSpecificProfile<ToiletProductDto, ToiletProductEntity>
	{
		public OperationResult UpdateSpecificInfo(ToiletProductDto newInfo, ToiletProductEntity entityToUpdate)
		{
			throw new NotImplementedException();
		}
	}
}
