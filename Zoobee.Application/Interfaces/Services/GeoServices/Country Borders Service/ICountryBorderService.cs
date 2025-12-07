using Zoobee.Domain.DataEntities.Environment.Geography;

namespace Zoobee.Application.Interfaces.Services.GeoServices
{
	public interface ICountryBorderService
	{
		bool IsPolygonInCountry(List<GeoPoint> polygon);
		bool IsPointInCountry(GeoPoint point);
		List<GeoPoint> GetCountryBorder();
	}

}
