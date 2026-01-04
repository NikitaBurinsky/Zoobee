using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Domain.DataEntities.Data_Primitives;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Application.DTOs.Products.Types
{
	public class FoodProductDto : BaseProductDto
	{
		public PetFoodType? FoodType { get; set; }
		public PetAgeRange? PetAgeRange { get; set; }
		public decimal? ProductWeightGrams { get; set; }
	}
}
