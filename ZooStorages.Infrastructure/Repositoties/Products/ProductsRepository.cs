using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStores.Infrastructure.Repositoties;

namespace ZooStorages.Infrastructure.Repositoties.Products
{
	public class ProductsRepository : RepositoryBase, IProductsRepository
    {
		public ProductsRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{}
		public IQueryable<ProductEntity> Products => dbContext.Products;

		public async Task<OperationResult<Guid>> CreateProductAsync(ProductEntity newProduct)
		{
			if (IsProductExists(newProduct.Name))
				return OperationResult<Guid>.Error(localizer["Error.Products.SimilarNameExists"], HttpStatusCode.BadRequest);
			newProduct.NormalizedName = NormalizeName(newProduct.Name);
			var entry = dbContext.Products.Add(newProduct);
			return entry != null ?
				OperationResult<Guid>.Success(entry.Entity.Id) :
				OperationResult<Guid>.Error(localizer["Error.Products.WriteDbError"], HttpStatusCode.InternalServerError);
		}

		public async Task<OperationResult> DeleteProductAbsoluteAsync(ProductEntity product)
		{
			if (IsProductExists(product.Id))
			{
				dbContext.Products.Remove(product);
				return OperationResult.Success();
			}
			else
				return OperationResult.Error(localizer["Error.Products.ProductNotFound"], HttpStatusCode.NotFound);
		}

		public async Task<ProductEntity> GetProductAsync(Guid Id)
		 => await dbContext.Products.FindAsync(Id);

		public async Task<ProductEntity> GetProductAsync(string ProductName)
		 => dbContext.Products.FirstOrDefault(e => e.NormalizedName == NormalizeName(ProductName));

		public async Task<List<ProductSlotEntity>> GetProductSellingSlotsAsync(ProductEntity product)
		{
			var entry = dbContext.Entry(product);
			if (entry == null)
				return null;
			await entry.Collection(e => e.SellingSlots).LoadAsync();
			return entry.Entity.SellingSlots.ToList();
		}

		public bool IsProductExists(Guid Id)
		 => dbContext.Products.Any(e => e.Id == Id);

		public bool IsProductExists(string ProductName)
		 => dbContext.Products.Any(e => e.NormalizedName == NormalizeName(ProductName));

		public async Task<OperationResult> UpdateProductAsync(ProductEntity productToUpdate, Action<ProductEntity> action)
		{
			var oldName = productToUpdate.Name;
			action(productToUpdate);
			if (oldName != productToUpdate.Name)
			{
				productToUpdate.NormalizedName = NormalizeName(productToUpdate.Name);
			}
			return OperationResult.Success();
		}

		private string NormalizeName(string name)
		{
			return name.Trim().ToLower();
		}
	}
}
