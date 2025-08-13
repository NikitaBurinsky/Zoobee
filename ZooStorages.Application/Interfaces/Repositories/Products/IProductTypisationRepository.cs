using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStorages.Domain.DataEntities.Products;
using ZooStorages.Core;
using ZooStores.Application.DtoTypes.Products.Categories;

namespace ZooStorages.Application.Interfaces.Repositories.Products
{
	public interface IProductTypisationRepository : IRepositoryBase
    {
		IQueryable<ProductTypeEntity> ProductTypes { get; }
		IQueryable<ProductCategoryEntity> ProductCategories { get; }
		/// <summary>
		/// Создает новую категорию продуктов (аксессуары, корма...)
		/// </summary>
		public Task<OperationResult<Guid>> CreateProductCategoryAsync(ProductCategoryEntity newCategory);
		/// <summary>
		/// Создает новый тип продуктов (ошейники, препараты от клещей...)
		/// </summary>
        public Task<OperationResult<Guid>> CreateProductTypeAsync(ProductTypeEntity newProductType);
		/// <summary>
		/// Возврат всех типов, относящихся к данной категории.
		/// </summary>
		public Task<OperationResult<List<ProductTypeEntity>>> GetTypesOfCategoryAsync(ProductCategoryEntity category);
		public Task<OperationResult<List<ProductEntity>>> GetProductsOfTypeAsync(ProductTypeEntity productTypeEntity);
		/// <summary>
		/// Получение категории по Id.
		/// </summary>
		public Task<ProductCategoryEntity> GetProductCategoryAsync(Guid Id);
		public Task<ProductCategoryEntity> GetProductCategoryAsync(string CategoryName);
		public Task<ProductTypeEntity> GetProductTypeAsync(Guid Id);
		public Task<ProductTypeEntity> GetProductTypeAsync(string TypeName);

		public bool IsCategoryExists(string categoryName);
		public bool IsCategoryExists(Guid Id);
		public bool IsTypeExists(string TypeName);
		public bool IsTypeExists(Guid Id);

		/// <summary>
		/// Позволяет удалить категорию, если та не содержит типов.
		/// </summary>
		public Task<OperationResult> DeleteProductCategoryAsync(ProductCategoryEntity category);
		/// <summary>
		/// Позволяет удалить тип, если тот не содержит продуктов.
		/// </summary>
		public Task<OperationResult> DeleteProductTypeAsync(ProductTypeEntity type);
		public Task<OperationResult> UpdateTypeAsync(ProductTypeEntity productTypeToUpdate, Action<ProductTypeEntity> action);
		public Task<OperationResult> UpdateCategoryAsync(ProductCategoryEntity productCategoryToUpdate, Action<ProductCategoryEntity> action);	
	}
}
