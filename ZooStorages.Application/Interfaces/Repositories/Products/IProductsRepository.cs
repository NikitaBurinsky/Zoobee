using ZooStorages.Core;
using ZooStorages.Domain.DataEntities.Products;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStores.Application.DtoTypes.Products.Categories;

namespace ZooStorages.Application.Interfaces.Repositories.Products
{
	public interface IProductsRepository : IRepositoryBase
    {
        IQueryable<ProductEntity> Products { get; }
    //Create
        public Task<OperationResult<Guid>> CreateProductAsync(ProductEntity newProduct);
    //Read
        public Task<ProductEntity> GetProductAsync(Guid Id);
		public Task<ProductEntity> GetProductAsync(string ProductName);
		public bool IsProductExists(Guid Id);
		public bool IsProductExists(string ProductName);
		public Task<List<ProductSlotEntity>> GetProductSellingSlotsAsync(ProductEntity product);
	//Update
		public Task<OperationResult> UpdateProductAsync(ProductEntity updatedProduct, Action<ProductEntity> action);
    //Delete
		/// <summary>
		/// Удаляет продукт вместе со всеми его слотами.
		/// </summary>
		public Task<OperationResult> DeleteProductAbsoluteAsync(ProductEntity product); 
    }
}
