using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.DTOs;
using ZooStorages.Domain.DataEntities.Products;
using ZooStores.Application.DtoTypes.Products.Delivery;

namespace ZooStorages.Application.Models.Catalog.Selling.DeliveryOptions
{
    public class DeliveryOptionDto : IPrimitiveDtoFromEntity<DeliveryOptionDto, DeliveryOptionEntity>
    {
        public Guid Id { get; set; }
        public List<Guid> ProductSlotIds { get; set; }
        public TimeSpan DeliveryTime { get; set; }
        public decimal DeliveryCost { get; set; }
        public List<string> PaymentTypes { get; set; }
        public static DeliveryOptionDto FromEntity(DeliveryOptionEntity entity)
        {
            return new DeliveryOptionDto
            {
                Id = entity.Id,
                ProductSlotIds = entity.ProductSlots.Select(e => e.Id).ToList(),
                DeliveryCost = entity.DeliveryCost,
                DeliveryTime = entity.DeliveryTime,
                PaymentTypes = entity.PaymentTypes.ToList(),
            };
        }
    }
}
