using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.DTOs;
using ZooStores.Application.DtoTypes.EntityComponents;

namespace ZooStorages.Application.Models.Components
{
    public class LocationDto : IPrimitiveDtoFromEntity<LocationDto, LocationEntity>
    {
        public Guid Id { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; } = "Belarus";
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public static LocationDto FromEntity(LocationEntity entity)
        {
            return new LocationDto
            {
                Id = entity.Id,
                Address = entity.Address,
                City = entity.City,
                Region = entity.Region,
                Country = entity.Country,
                Longitude = entity.Longitude,
                Latitude = entity.Latitude,
            };
        }
    }
}
