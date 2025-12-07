using Zoobee.Application.DTOs.Business_Items.Base;

namespace Zoobee.Application.DTOs.Catalog
{
	public class TagDto : BaseEntityItemDto
	{
		public Guid Id { get; set; }
		public string TagName { get; set; }
		public string ProductType { get; set; }
	}
}
