using Zoobee.Application.DTOs.Business_Items.Base;

namespace Zoobee.Application.DTOs.Catalog
{
	public class ReviewDto : BaseEntityItemDto
	{
		public Guid Id { get; set; }
		public Guid ReviewerUserId { get; set; }
		public Guid ReviewedProductId { get; set; }
		public float Rating { get; set; }
		public string Text { get; set; }
	}
}
