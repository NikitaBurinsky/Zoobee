using Zoobee.Application.Shared.DTOs.Filters.Products.Base_Product;
using Zoobee.Domain.DataEntities.Data_Primitives;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Application.Shared.DTOs.Filters
{
	public class FoodProductFilterDto : BaseProductFilterDto
	{
		public List<PetFoodType>? FoodTypes { get; set; }
		public PetAgeRange? PetAgeRange { get; set; }
		public ProductWeigthGrammsRange ProductWeightGrammsRange { get; set; }
		public record ProductWeigthGrammsRange(decimal Min, decimal Max);
	}
}
