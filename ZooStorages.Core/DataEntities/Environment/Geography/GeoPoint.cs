using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoobee.Domain.DataEntities.Environment.Geography
{
    public record GeoPoint
    {
		public double Longitude { get; set; }
		public double Latitude { get; set; }

		public GeoPoint(double longitude, double latitude)
		{
			Longitude = longitude;
			Latitude = latitude;
		}
	}
}
