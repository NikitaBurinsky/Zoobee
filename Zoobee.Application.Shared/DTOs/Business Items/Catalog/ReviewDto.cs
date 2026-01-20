using Zoobee.Application.Shared.DTOs.Business_Items.Base;

namespace Zoobee.Application.Shared.DTOs.Catalog
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
