using Zoobee.Application.DTOs.Environtment.Geography;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Environment.Geography;

namespace Zoobee.Application.Interfaces.Services.GeoServices.GeoLocationService
{
	public interface IGeoLocationService
	{
		public Task<OperationResult<Guid>> CreateDeliveryArea(DeliveryAreaDto points);
		public Task<OperationResult<Guid>> CreateLocation(LocationDto loc);
		public Task<OperationResult<Guid>> CreateLocation(GeoPoint geoPoint);
		public Task<OperationResult<Guid>> CreateLocation(string city, string address);
		public bool IsLocationInArea(LocationDto location, Guid deliveryArea);
		public bool IsLocationInArea(GeoPoint location, Guid deliveryArea);
		public Task<bool> IsLocationInArea(string city, string address, Guid deliveryArea);
	}
}
