using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Application.Interfaces.Services.Products.ProductsStorage
{
	public interface IProductsStorageService
	{
		public Task<OperationResult<Guid>> CreateProductAndSave<ProductDto, ProductEntity>(ProductDto dto)
			where ProductDto : BaseProductDto
			where ProductEntity : BaseProductEntity;
	}
}
