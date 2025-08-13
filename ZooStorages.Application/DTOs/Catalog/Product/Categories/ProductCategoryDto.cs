using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.DTOs;
using ZooStores.Application.DtoTypes.Products.Categories;

namespace ZooStorages.Application.Models.Catalog.Product.Categories
{
    public class ProductCategoryDto : IPrimitiveDtoFromEntity<ProductCategoryDto, ProductCategoryEntity>
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public static ProductCategoryDto FromEntity(ProductCategoryEntity entity)
        {
            return new ProductCategoryDto
            {
                CategoryName = entity.CategoryName,
                Description = entity.Description,
            };
        }
    }
}
