using Zoobee.Application.DTOs.Business_Items.Base;

namespace Zoobee.Application.DTOs.Environtment.Manufactures
{
	public class ProductLineupDto : BaseEntityItemDto
	{
		public Guid? Id { get; set; }
		public string BrandName { get; set; }
		public string LineupName { get; set; }
		public string LineupDescription { get; set; }
	}
}
