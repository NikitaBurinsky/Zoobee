using Zoobee.Application.Interfaces.Repositories.IRepositoryBase;
using Zoobee.Domain.DataEntities.Environment.Geography;

namespace Zoobee.Application.Interfaces.Repositories.Environtment.Geography
{
	public interface ILocationsRepository : IRepositoryBase<LocationEntity>
	{
		public LocationEntity GetByAddress(string city, string address);
		public LocationEntity GetByGeoPoint(GeoPoint geoPoint);
	}
}