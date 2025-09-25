using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Application.DtoTypes.Base;
using static Zoobee.Domain.DataEntities.Environment.Geography.DeliveryAreaEntity;

namespace Zoobee.Domain.DataEntities.Environment.Geography
{
    public class LocationEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string? Address { get; set; }
		public string? NormalizedAddress { get; set; }
        public string? City { get; set; }
		public string? NormalizedCity { get; set; } 
        public GeoPoint GeoPoint { get; set; }
		public LocationEntity() {}
		public LocationEntity(string? address, string? city, GeoPoint geoPoint)
		{
			Address = address;
			City = city;
			GeoPoint = geoPoint;
		}
	}

    public class LocationEntityConfigurator : IEntityTypeConfiguration<LocationEntity>
    {
        public void Configure(EntityTypeBuilder<LocationEntity> builder)
        {
            builder.OwnsOne(x => x.GeoPoint);
        }
    }
}
