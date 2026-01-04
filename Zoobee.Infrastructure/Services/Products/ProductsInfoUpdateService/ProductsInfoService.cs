using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork;
using Zoobee.Application.Interfaces.Services.Products.Catalog.ProductsInfoService;
using Zoobee.Application.Interfaces.Services.Products.ProductsStorage;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.Services.Products.Matching;

namespace Zoobee.Infrastructure.Services.Products.ProductsInfoService
{
	public class ProductsInfoService : IProductsInfoService
	{
		IProductsUnitOfWork uof;
		ProductInfoMatcher productsInfoMatcher;
		IProductsStorageService ProductsStorageService;
		IServiceCollection services;

		public OperationResult UpdateOrAddProductInfo<Entity, Dto>(Dto dto, 
			string sourceUrl = null)
			where Entity : BaseProductEntity
			where Dto : BaseProductDto
		{
			Entity entity = null;

			if (sourceUrl != null)
				entity = (Entity)uof.AllProducts.GetAll()
					.Include(e => e.SellingSlots)
					.Where(e =>
						e.SellingSlots
						.Any(s => s.SellingUrl == sourceUrl))
					.FirstOrDefault();

			if (entity == null)
				entity = (Entity)productsInfoMatcher.FindMatchAsync(dto).Result;

			if (entity == null)
			{
				//Create New Product
			}
			else
			{
				//Update Product 
				//Но написать надо так, что бы на базе этого метода можно было апдейтнуть любой другой тип продукта
			}
			throw new NotImplementedException();
		}







	}
}
