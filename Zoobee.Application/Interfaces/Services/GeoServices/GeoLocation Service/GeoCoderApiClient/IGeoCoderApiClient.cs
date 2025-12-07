using Zoobee.Application.DTOs.Environtment.Geography;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Environment.Geography;

namespace Zoobee.Application.Interfaces.Services.GeoServices.GeoLocationService.GeoCoderApiClient
{
	public interface IGeoCoderApiClient
	{
		public Task<OperationResult<LocationDto>> GetLocationByAddressAsync(string address);
		public Task<OperationResult<LocationDto>> GetLocationByGeoPoint(GeoPoint geoPoint);

	}
}
