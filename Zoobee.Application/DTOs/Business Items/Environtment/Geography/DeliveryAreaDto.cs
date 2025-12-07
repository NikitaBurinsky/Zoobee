using Zoobee.Application.DTOs.Business_Items.Base;
using Zoobee.Domain.DataEntities.Environment.Geography;

namespace Zoobee.Application.DTOs.Environtment.Geography
{
	public class DeliveryAreaDto : BaseEntityItemDto
	{
		public Guid? Id { get; set; }
		public string? AreaName { get; set; }
		public List<GeoPoint> GeoArea { get; set; }
		public bool IsTemplate { get; set; }
		public string? SellerCompanyName { get; set; }
	}
}
