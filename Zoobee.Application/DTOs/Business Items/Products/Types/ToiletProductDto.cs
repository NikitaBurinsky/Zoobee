using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Domain.DataEntities.Data_Primitives;
using Zoobee.Domain.DataEntities.Products.ToiletProductEntity;

namespace Zoobee.Application.DTOs.Products.Types
{
	public class ToiletProductDto : BaseProductDto
	{
		public ToiletType ToiletType { get; set; }
		public SizeDimensions? SizeDimensions { get; set; }
		public float? VolumeLiters { get; set; }
		public PetAgeRange? petAgeRange { get; set; }
	}
}
