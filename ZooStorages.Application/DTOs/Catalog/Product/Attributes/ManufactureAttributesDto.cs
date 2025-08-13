using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.DTOs;
using ZooStorages.Domain.DataEntities.Products.Components.Attributes;

namespace ZooStorages.Application.Models.Catalog.Product.Attributes
{
    public class ManufactureAttributesDto : IPrimitiveDtoFromEntity<ManufactureAttributesDto, ManufactureAttributes>
    {
        public string Brand { get; set; }
        public string? CreatorCountry { get; set; }
        /// <summary>
        /// Длина UPC кода - 12
        /// </summary>
        public string UPC_Code { get; set; }
        /// <summary>
        /// Длина EAN кода - 13
        /// </summary>
        public string EAN_Code { get; set; }

        public static ManufactureAttributesDto FromEntity(ManufactureAttributes entity)
        {
            return new ManufactureAttributesDto
            {
                Brand = entity.Brand,
                CreatorCountry = entity.CreatorCountry,
                EAN_Code = entity.EAN_Code,
                UPC_Code = entity.UPC_Code,
            };
        }
    }
}
