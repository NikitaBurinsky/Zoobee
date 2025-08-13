using ZooStorages.Application.Models.Catalog.Product.Product;
using ZooStorages.Core;

namespace ZooStorages.Application.Interfaces.Services.ProductsAdminitrator
{
	public interface IProductsAdministratorService
    {
        public Task<OperationResult<Guid>> CreateProductAsync(ProductDto productDto);
        public OperationResult<List<ProductDto>> ListProductsUnordered(int pagenum, int pagesize);
    }
}
