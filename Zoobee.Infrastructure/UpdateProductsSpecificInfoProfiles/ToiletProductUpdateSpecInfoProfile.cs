using Zoobee.Application.Shared.DTOs.Products.Types;
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
