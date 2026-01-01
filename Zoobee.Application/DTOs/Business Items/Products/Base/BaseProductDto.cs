using Zoobee.Application.DTOs.Business_Items.Base;

namespace Zoobee.Application.DTOs.Products.Base
{
	public class BaseProductDto : BaseEntityItemDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Dictionary<string, string> SiteArticles { get; set; } //Прим. Zoobazar - https://...product-url/
		public string UPC { get; set; }
		public string EAN { get; set; }
		public float Rating { get; set; }
		public decimal MinPrice { get; set; }
		public decimal MaxPrice { get; set; }
		public string CreatorCountryName { get; set; }
		public string BrandName { get; set; }
		public string ProductLineupName { get; set; }
		public string CreatorCompanyName { get; set; }
		public List<string> Tags { get; set; }
		public string PetKind { get; set; }
		public List<string> MediaURIs { get; set; }

		/*
		 * Слоты продажи, и комментарии подгружаются отдельно
		 */
	}
}
