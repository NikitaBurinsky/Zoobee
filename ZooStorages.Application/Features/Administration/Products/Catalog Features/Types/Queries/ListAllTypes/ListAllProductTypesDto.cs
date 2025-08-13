using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.DTOs;
using ZooStorages.Domain.DataEntities.Products;

namespace ZooStorages.Application.Features.Products.Catalog_Features.Types.Queries.ListAllTypes
{
	public class ListAllProductTypesDto : IPrimitiveDtoFromEntity<ListAllProductTypesDto, ProductTypeEntity>
	{
		public string Name { get; set; }
		public string Information { get; set; }
		public string Category { get; set; }

		public static ListAllProductTypesDto FromEntity(ProductTypeEntity entity)
		{
			return new ListAllProductTypesDto()
			{
				Name = entity.Name,
				Information = entity.Information,
				Category = entity.Category.CategoryName,
			};
		}
	}
}
