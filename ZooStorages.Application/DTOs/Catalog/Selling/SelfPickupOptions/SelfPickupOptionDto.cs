using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.DTOs;
using ZooStorages.Application.Models.Components;
using ZooStorages.Domain.DataEntities.Products;
using ZooStores.Application.DtoTypes.EntityComponents;
using ZooStores.Application.DtoTypes.Products.Delivery;

namespace ZooStorages.Application.Models.Catalog.Selling.SelfPickupOptions
{
    public class SelfPickupOptionDto : IPrimitiveDtoFromEntity<SelfPickupOptionDto, SelfPickupOptionEntity>
    {
        public Guid Id { get; set; }
        public LocationDto PickupPointLocation { get; set; }
        public uint? AvaibableInPlace { get; set; }
        public TimeSpan DeliveryToPointTime { get; set; }
        public decimal DeliveryToPointCost { get; set; }
        public List<Guid> ProductSlotIds { get; set; }
        public ICollection<string> PaymentTypes { get; set; }
        public bool IsAvaibableToBook { get; set; }

        public static SelfPickupOptionDto FromEntity(SelfPickupOptionEntity entity)
        {
            return new SelfPickupOptionDto
            {
                Id = entity.Id,
                PickupPointLocation = LocationDto.FromEntity(entity.PickupPointLocation),
                AvaibableInPlace = entity.AvaibableInPlace,
                DeliveryToPointCost = entity.DeliveryToPointCost,
                DeliveryToPointTime = entity.DeliveryToPointTime,
                PaymentTypes = entity.PaymentTypes,
                IsAvaibableToBook = entity.IsAvaibableToBook,
            };
        }
    }
}
