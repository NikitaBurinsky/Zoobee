using Microsoft.Extensions.Configuration;
using Zoobee.Application.Interfaces.Services.GeoServices;
using Zoobee.Domain.DataEntities.Environment.Geography;
using Zoobee.Infrastructure.Services.GeoServices.Country_Borders_Service.PolygonGeometryChecker;

namespace Zoobee.Infrastructure.Services.GeoServices.CountryBordersService
{
	public class CountryBorderService : ICountryBorderService
	{
		private readonly List<GeoPoint> _countryBorder;
		private readonly IConfiguration _config;
		public CountryBorderService(IConfiguration configuration)
		{
			_config = configuration;
			_countryBorder = LoadBordersFromConfig(configuration);
		}

		public bool IsPolygonInCountry(List<GeoPoint> polygon)
		{
			return PolygonContainsChecker.IsPolygonCompletelyInside(polygon, _countryBorder);
		}

		public bool IsPointInCountry(GeoPoint point)
		{

			return PolygonContainsChecker.IsPointInPolygon(point, _countryBorder);
		}

		private List<GeoPoint> LoadBordersFromConfig(IConfiguration configuration)
		{
			var borders = configuration.GetSection("CountryBorders:BY").Get<List<GeoPoint>>();

			if (borders == null || borders.Count == 0)
			{
				throw new ArgumentNullException("TODO Config BY Borders null : 7126");
			}
			return borders;
		}
		public List<GeoPoint> GetCountryBorder()
		{
			return _countryBorder;
		}
	}
}
