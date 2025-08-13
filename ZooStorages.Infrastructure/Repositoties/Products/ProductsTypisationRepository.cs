using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStores.Application.DtoTypes.Products.Categories;
using ZooStores.Infrastructure.Repositoties;

namespace ZooStorages.Infrastructure.Repositoties.Products
{
	public class ProductsTypisationRepository : RepositoryBase, IProductTypisationRepository
	{
		public ProductsTypisationRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{}
		public IQueryable<ProductCategoryEntity> ProductCategories => dbContext.ProductCategories;

		public IQueryable<ProductTypeEntity> ProductTypes => dbContext.ProductTypes;

		public async Task<OperationResult<Guid>> CreateProductCategoryAsync(ProductCategoryEntity newCategory)
		{
			if (newCategory.CategoryName == null) 
				return OperationResult<Guid>.Error(localizer["Error.ProductCategories.InvalidName"], HttpStatusCode.BadRequest);
			if(IsCategoryExists(newCategory.CategoryName))
				return OperationResult<Guid>.Error(localizer["Error.ProductCategories.SimilarNameExists"], HttpStatusCode.BadRequest);
			var entry = await dbContext.ProductCategories.AddAsync(newCategory);
			if(entry == null)
				return OperationResult<Guid>.Error(localizer["Error.ProductCategories.WriteDbFailure"]
					,HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(entry.Entity.Id);
		}

		public async Task<OperationResult<Guid>> CreateProductTypeAsync(ProductTypeEntity newProductType)
		{
			if(IsCategoryExists(newProductType.Category.Id))
			{
				var category = ProductCategories.FirstOrDefault(e => e.Id == newProductType.Category.Id);
				newProductType.Category = category;
				return IsTypeExists(newProductType.Name) ?
					OperationResult<Guid>.Error(localizer["Error.ProductTypes.SimilarNameExists"], HttpStatusCode.BadRequest) 
		 		:	OperationResult<Guid>.Success(dbContext.ProductTypes.Add(newProductType).Entity.Id);
			}
			else
				return OperationResult<Guid>.Error(localizer["Error.ProductTypes.CategoryNotFound"], HttpStatusCode.BadRequest);
		}

		/// <summary>
		/// Позволяет удалить категорию, если та не содержит типов.
		/// </summary>
		public async Task<OperationResult> DeleteProductCategoryAsync(ProductCategoryEntity category)
		{
			if (dbContext.ProductTypes.Any(e => e.Category.Id == category.Id))
				return OperationResult.Error(localizer["Error.ProductCategories.CantDeleteWhileContainsTypes"], HttpStatusCode.BadRequest);
			var res = dbContext.Remove(category);
			return OperationResult.Success();
		}
		/// <summary>
		/// Позволяет удалить тип, если тот не содержит продуктов.
		/// </summary>
		public async Task<OperationResult> DeleteProductTypeAsync(ProductTypeEntity type)
		{
			if (dbContext.Products.Any(e => e.ProductType.Id == type.Id))
				return OperationResult.Error(localizer["Error.ProductCategories.CantDeleteWhileContainsProducts"], HttpStatusCode.BadRequest);
			var res = dbContext.Remove(type);
			return OperationResult.Success();
		}

		public async Task<ProductCategoryEntity> GetProductCategoryAsync(Guid Id)
			=> dbContext.ProductCategories.FirstOrDefault(e => e.Id == Id);

		public async Task<ProductCategoryEntity> GetProductCategoryAsync(string CategoryName)
			=> dbContext.ProductCategories.FirstOrDefault(e => e.CategoryName == CategoryName);

		public async Task<OperationResult<List<ProductEntity>>> GetProductsOfTypeAsync(ProductTypeEntity productTypeEntity)
		{
			if(IsTypeExists(productTypeEntity.Id))
			{
				var entry = dbContext.Entry(productTypeEntity);
				if(entry != null)
				{
					await entry.Collection(e => e.ProductsOfType).LoadAsync();
					return OperationResult<List<ProductEntity>>.Success(entry.Entity.ProductsOfType.ToList());
				}
				return OperationResult<List<ProductEntity>>.Error(localizer["Error.ProductTypes.ReadDbError"], HttpStatusCode.InternalServerError);
			}
			return OperationResult<List<ProductEntity>>.Error(localizer["Error.ProductTypes.ProductTypeNotFound"], HttpStatusCode.NotFound);
		}

		public async Task<ProductTypeEntity> GetProductTypeAsync(Guid Id)
			=> dbContext.ProductTypes.FirstOrDefault(e => e.Id == Id);

		public async Task<ProductTypeEntity> GetProductTypeAsync(string TypeName)
			=> dbContext.ProductTypes.FirstOrDefault(e => e.Name == TypeName);

		public async Task<OperationResult<List<ProductTypeEntity>>> GetTypesOfCategoryAsync(ProductCategoryEntity category)
		{
			var entry = dbContext.Entry(category);
			if(entry == null)
				return OperationResult<List<ProductTypeEntity>>.Error(localizer["Error.ProductTypes.CategoryNotFound"], HttpStatusCode.NotFound);
			await entry.Collection(e => e.ProductTypes).LoadAsync();
			return OperationResult<List<ProductTypeEntity>>.Success(entry.Entity.ProductTypes.ToList());
		}

		public bool IsCategoryExists(string categoryName)
		 => dbContext.ProductCategories.Any(e => e.CategoryName == categoryName);

		public bool IsCategoryExists(Guid Id)
		 => dbContext.ProductCategories.Any(e => e.Id == Id);
		public bool IsTypeExists(string TypeName)
		 => dbContext.ProductTypes.Any(e => e.Name == TypeName);
		public bool IsTypeExists(Guid Id)
		 => dbContext.ProductTypes.Any(e => e.Id == Id);

		public async Task<OperationResult> UpdateCategoryAsync(ProductCategoryEntity productCategoryToUpdate, Action<ProductCategoryEntity> action)
		{
			var entry = dbContext.ProductCategories.Update(productCategoryToUpdate);
			if(entry == null)
				return OperationResult.Error(localizer["Error.ProductCategories.UpdateDbFailure"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();			
		}

		public async Task<OperationResult> UpdateTypeAsync(ProductTypeEntity productTypeToUpdate, Action<ProductTypeEntity> action)
		{
			var entry = dbContext.ProductTypes.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.ProductCategories.UpdateDbFailure"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}
	}
}
