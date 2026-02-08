using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork;
using Zoobee.Application.Interfaces.Services.Products.Catalog.ProductsInfoService;
using Zoobee.Application.Interfaces.Services.Products.ProductsStorage;
using Zoobee.Application.Interfaces.Services.ProductTypeRegistry;
using Zoobee.Application.Shared.DTOs.Products.Base;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.Services.Products.Matching;
using Zoobee.Infrastructure.UpdateProductsSpecificInfoProfiles;

namespace Zoobee.Infrastructure.Services.Products.ProductsInfoService
{
	public class ProductsInfoService : IProductsInfoService
	{
		IProductsUnitOfWork uof;
		ProductInfoMatcher productsInfoMatcher;
		IProductsStorageService ProductsStorageService;
		IServiceProvider services;
		private readonly IProductTypeRegistryService _productRegistry;


		public ProductsInfoService(IProductsUnitOfWork uof, ProductInfoMatcher productsInfoMatcher, 
			IProductsStorageService productsStorageService, IServiceProvider services,
			IProductTypeRegistryService productTypeRegistryService)
		{
			this.uof = uof;
			_productRegistry = productTypeRegistryService;
			this.productsInfoMatcher = productsInfoMatcher;
			ProductsStorageService = productsStorageService;
			this.services = services;
		}

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
				var res = ProductsStorageService.CreateProductAndSave<Dto, Entity>(dto).Result;
				if (res.Failed)
					return res;
			}
			else
			{
				var baseProductUpdater = services.GetRequiredService<IUpdateProductSpecificProfile<BaseProductDto, BaseProductEntity>>();
				var specProductUpdater = services.GetRequiredService<IUpdateProductSpecificProfile<Dto, Entity>>();
				if(baseProductUpdater == null || specProductUpdater == null)
				{
					string dtoTypeName = nameof(Dto);
					string entityTypeName = nameof(Entity);
					return OperationResult.Error($"TODO Описание - крч сервисы не конструируются жоско в шаблонном методе. Хуйню передали. | " +
						$"ProductDtoType : {dtoTypeName} \nEntityTypeName : {entityTypeName}", HttpStatusCode.BadRequest);
				}


				var res = baseProductUpdater.UpdateSpecificInfo(dto, entity);
				if (res.Failed)
					return res;
				res = specProductUpdater.UpdateSpecificInfo(dto, entity);
				return res;
			}
			return OperationResult.Success();
		}

		public OperationResult UpdateOrAddProductInfoOfType(BaseProductDto dto, Type productType, string sourceUrl = null) 
		{
			var mapping = _productRegistry.GetMappingOrDefault(productType);

			var method = typeof(IProductsInfoService).GetMethod(nameof(IProductsInfoService.UpdateOrAddProductInfo));
			var genericMethod = method.MakeGenericMethod(mapping.EntityType, mapping.DtoType);

			var res = (OperationResult)genericMethod.Invoke(this, new object[] { dto, sourceUrl });
			throw new NotImplementedException();
		}
	}
}
