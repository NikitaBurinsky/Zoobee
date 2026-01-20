using Zoobee.Application.Shared.DTOs.Business_Items.Base;

namespace Zoobee.Application.Shared.DTOs.Catalog
{
	public class TagDto : BaseEntityItemDto
	{
		public Guid Id { get; set; }
		public string TagName { get; set; }
		public string ProductType { get; set; }
	}
}
