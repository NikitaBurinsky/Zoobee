using Zoobee.Domain.Enums;

namespace Zoobee.Application.DTOs.Filters.Products.Base_Product
{
	public class BaseProductFilterDto
	{
		public RatingsRange? RatingRange { get; set; }
		public PriceRange? Price { get; set; }
		public List<string>? FromCountries { get; set; }
		public List<string>? FromBrands { get; set; }
		public List<string>? ProductLineups { get; set; }
		public List<string>? CreatorCompanies { get; set; }
		public List<string>? Tags { get; set; }
		public List<string>? PetKinds { get; set; }
		public OrderingType OrderingType { get; set; }
		public record PriceRange(decimal Min, decimal Max);
		public record RatingsRange(float Min, float Max);
	}
}
