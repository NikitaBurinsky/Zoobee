using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.DTOs;
using ZooStorages.Domain.DataEntities.Products.Components.Attributes;
using ZooStorages.Domain.DataEntities.Products.Components.Dimensions;
using ZooStorages.Domain.Enums;

namespace ZooStorages.Application.Models.Catalog.Product.Attributes
{
    public class PhysicalAttributesDto : IPrimitiveDtoFromEntity<PhysicalAttributesDto, PhysicalAttributes>
    {
        public ProductDimensions? Dimensions { get; set; }
        public List<string>? Materials { get; set; }
        public decimal? WeightOfProducts { get; set; }
        public string? Color { get; set; }
        public uint ContentMeasurementsUnits { get; set; }

        public static PhysicalAttributesDto FromEntity(PhysicalAttributes entity)
        {
            return new PhysicalAttributesDto
            {
                Dimensions = entity.Dimensions,
                Materials = entity.Materials,
                WeightOfProducts = entity.WeightOfProducts,
                Color = entity.Color,
                ContentMeasurementsUnits = (uint)entity.ContentMeasurementsUnits
            };
        }

    }
}
