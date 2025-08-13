using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.DTOs;
using ZooStorages.Domain.DataEntities.Products;

namespace ZooStorages.Application.Models.Catalog.Product.Types
{
    public class ProductTypeDto : IPrimitiveDtoFromEntity<ProductTypeDto, ProductTypeEntity>
    {
        public string TypeName { get; set; }
        public string Information { get; set; }
        public string Category { get; set; }

        public static ProductTypeDto FromEntity(ProductTypeEntity entity)
        {
            return new ProductTypeDto
            {
                TypeName = entity.Name,
                Information = entity.Information,
                Category = entity.Category.CategoryName
            };
        }
    }
}
