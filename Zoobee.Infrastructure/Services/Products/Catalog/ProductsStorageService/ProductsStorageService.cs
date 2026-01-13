using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork;
using Zoobee.Application.Interfaces.Services.MappingService;
using Zoobee.Application.Interfaces.Services.Products.ProductsStorage;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.Services.Products.Catalog.ProductsStorageService
{
	public class ProductsStorageService : IProductsStorageService
	{
		IMappingService mapper;
		IProductsUnitOfWork productsUoW;

		public ProductsStorageService(IMappingService mapper, IProductsUnitOfWork productsUoW)
		{
			this.mapper = mapper;
			this.productsUoW = productsUoW;
		}

		public async Task<OperationResult<Guid>> CreateProductAndSave<ProductDto, ProductEntity>(ProductDto dto)
			where ProductDto : BaseProductDto
			where ProductEntity : BaseProductEntity
		{
			var mapRes = mapper.Map<ProductDto, ProductEntity>(dto);
			if (mapRes.Failed)
				return OperationResult<Guid>.Error(mapRes.Message, mapRes.ErrCode);
			var repos = productsUoW.RepositoryOfType<ProductEntity>();
			var result = await repos.CreateAsync(mapRes.Returns);
			if (result.Succeeded)
			{
				await repos.SaveChangesAsync();
				return result;
			}
			return result;
		}
	}
}
