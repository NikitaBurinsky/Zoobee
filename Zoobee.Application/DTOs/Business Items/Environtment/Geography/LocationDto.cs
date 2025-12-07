using Zoobee.Application.DTOs.Business_Items.Base;
using Zoobee.Domain.DataEntities.Environment.Geography;

namespace Zoobee.Application.DTOs.Environtment.Geography
{
	public class LocationDto : BaseEntityItemDto
	{
		public string? Address { get; set; }
		public string? City { get; set; }
		public GeoPoint GeoPoint { get; set; }
	}
}
