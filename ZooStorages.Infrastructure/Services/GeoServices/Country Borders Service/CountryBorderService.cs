using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.Interfaces.Services.GeoServices;
using Zoobee.Domain.DataEntities.Environment.Geography;

namespace Zoobee.Infrastructure.Services.GeoServices.CountryBordersService
{
	public class CountryBorderService : ICountryBorderService
	{
		private readonly List<GeoPoint> _countryBorder;
		private readonly IConfiguration _config;
		public CountryBorderService(IConfiguration configuration)
		{
			_config = configuration;
		}

		public bool IsPolygonInCountry(ICollection<GeoPoint> polygon)
		{
			foreach (var point in polygon)
			{
				if (!IsPointInPolygon(_countryBorder, point))
				{
					return false;
				}
			}
			return true;
		}

		public bool IsPointInCountry(GeoPoint point)
		{

			return IsPointInPolygon(_countryBorder, point);
		}

		private List<GeoPoint> LoadBordersFromConfig(IConfiguration configuration)
		{
			var borders = new List<GeoPoint>();
			
			var borderConfig = configuration.GetSection("CountryBorders:BY").Get<List<GeoPoint>>();

			if (borders == null)
			{
				throw new ArgumentNullException("TODO Config BY Borders null : 7126");
			}
			return borders;
		}

		// Алгоритм проверки точки в полигоне (Ray Casting)
		private bool IsPointInPolygon(List<GeoPoint> polygon, GeoPoint testPoint)
		{
			if (polygon == null || polygon.Count < 3)
				return false;

			bool inside = false;
			int n = polygon.Count;

			for (int i = 0, j = n - 1; i < n; j = i++)
			{
				if (((polygon[i].Latitude > testPoint.Latitude) != (polygon[j].Latitude > testPoint.Latitude)) &&
					(testPoint.Longitude < (polygon[j].Longitude - polygon[i].Longitude) *
					 (testPoint.Latitude - polygon[i].Latitude) /
					 (polygon[j].Latitude - polygon[i].Latitude) + polygon[i].Longitude))
				{
					inside = !inside;
				}
			}

			return inside;
		}

		public List<GeoPoint> GetCountryBorder()
		{
			return _countryBorder;
		}
	}
}
