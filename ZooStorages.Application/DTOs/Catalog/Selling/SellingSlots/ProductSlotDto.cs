using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.DTOs;
using ZooStorages.Domain.DataEntities.Products;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStores.Application.DtoTypes.Products.Delivery;

namespace ZooStorages.Application.Models.Catalog.Selling.SellingSlots
{
    public class ProductSlotDto : IPrimitiveDtoFromEntity<ProductSlotDto, ProductSlotEntity>
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public decimal DefaultSlotPrice { get; set; }
        public decimal Discount { get; set; }
        public bool IsAvaibable { get; set; }
        public ICollection<Guid> DeliveryOptions { get; set; }
        public ICollection<Guid> SelfPickupOptions { get; set; }

        public static ProductSlotDto FromEntity(ProductSlotEntity entity)
        {
            return new ProductSlotDto
            {
                Id = entity.Id,
                ProductId = entity.Product.Id,
                DefaultSlotPrice = entity.DefaultSlotPrice,
                Discount = entity.Discount,
                DeliveryOptions = entity.DeliveryOptions.Select(x => x.Id).ToList(),
                SelfPickupOptions = entity.SelfPickupOptions.Select(x => x.Id).ToList(),
                IsAvaibable = entity.IsAvaibable,
            };
        }
    }
}
