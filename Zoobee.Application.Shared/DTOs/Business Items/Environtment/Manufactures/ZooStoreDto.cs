using Zoobee.Application.Shared.DTOs.Business_Items.Base;
using Zoobee.Application.Shared.DTOs.Environment.Geography;
using Zoobee.Domain.DataEntities.Environment.Manufactures;

namespace Zoobee.Application.Shared.DTOs.Environment.Manufactures
{
	public class ZooStoreDto : BaseEntityItemDto
	{
		public Guid? Id { get; set; }
		public ZooStoreType StoreType { get; set; }
		public LocationDto? Location { get; set; }
		public string SellerCompanyName { get; set; }
		public string Name { get; set; }
		public TimeOnly? OpeningTime { get; set; }
		public TimeOnly? ClosingTime { get; set; }
	}
}
