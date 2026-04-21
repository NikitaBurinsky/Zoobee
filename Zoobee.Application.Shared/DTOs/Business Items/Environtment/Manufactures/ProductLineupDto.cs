using Zoobee.Application.Shared.DTOs.Business_Items.Base;

namespace Zoobee.Application.Shared.DTOs.Environment.Manufactures
{
	public class ProductLineupDto : BaseEntityItemDto
	{
		public Guid? Id { get; set; }
		public string BrandName { get; set; }
		public string LineupName { get; set; }
		public string LineupDescription { get; set; }
	}
}
