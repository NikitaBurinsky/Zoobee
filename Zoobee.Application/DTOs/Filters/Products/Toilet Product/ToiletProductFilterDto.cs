using Zoobee.Application.DTOs.Filters.Products.Base_Product;
using Zoobee.Domain.DataEntities.Data_Primitives;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Application.DTOs.Filters.Products.Toilet_Product
{
	public class ToiletProductFilterDto : BaseProductFilterDto
	{
		public List<ToiletType>? ToiletTypes { get; set; }
		public SizeDimensionsRange? DimensionsRange { get; set; }
		public VolumeRange? VolumeLitersRange { get; set; }
		public PetAgeRange? PetAgeRange { get; set; }

		public record SizeDimensionsRange(SizeDimensions Min, SizeDimensions Max);
		public record VolumeRange(float Min, float Max);
	}
}
