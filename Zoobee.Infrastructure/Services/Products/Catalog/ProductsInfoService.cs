using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Application.Interfaces.Services.Products.Catalog.ProductsInfoService;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.Services.Products.Matching;
using Zoobee.Infrastructure.Services.Products.ProductsStorage;

namespace Zoobee.Infrastructure.Services.Products.Catalog
{
	public class ProductsInfoService : IProductsInfoService
	{
		IBaseProductsRepository baseProductsRepository;
		Zoobee.Infrastructure.Services.Products.Matching.ProductInfoMatcher productsInfoMatcher;
		public ProductsInfoService(
			IBaseProductsRepository baseProductsRepository) 
		{
			this.baseProductsRepository = baseProductsRepository; 
		}
		public OperationResult UpdateAddProductInfo(BaseProductDto dto)
		{
			throw new NotImplementedException();
		}

		public OperationResult UpdateAddProductInfo(FoodProductDto dto)
		{
			throw new NotImplementedException();
		}
	}
}
