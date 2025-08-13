using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStorages.Domain.DataEntities.Products.Components.DynamicAttributes;
using ZooStorages.Domain.DataEntities.Products.Components.ExtendedAttributes;
using ZooStores.Infrastructure.Repositoties;

namespace ZooStorages.Infrastructure.Repositoties
{
	public class DynamicAttributesRepository : RepositoryBase, IDynamicAttributesRepository
	{
		public DynamicAttributesRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<ProductAttributeEntity> AttributesValues => dbContext.ExtAttributes;
		public IQueryable<ProductAttributeTypeEntity> AttributeTypes => dbContext.ExtAttributesTypes;

		/// <summary>
		/// Создает в бд атрибуты соответствующих типов. Если тип отсутствует, он будет создан
		/// </summary>
		public async Task<ICollection<ProductAttributeEntity>> CreateAttributesValuesAsync(Dictionary<string, string> attributes)
		{
			List<ProductAttributeEntity> attributesValues = new List<ProductAttributeEntity>();
			foreach(var pair in attributes)
			{
				if(!IsAttributeTypeExists(pair.Key))
				{
					var res = await CreateAttributeTypeAsync(new ProductAttributeTypeEntity
					{
						AttributeName = pair.Key,
					});
					if (res.Failed)
						continue;
					await SaveChangesAsync();
				}
				var attribute = new ProductAttributeEntity
				{
					AttributeType = await GetAttributeTypeEntityAsync(pair.Key),
					AttributeValue = pair.Value,
				};
				var addresult = await CreateAttributeValueAsync(attribute);//TODO Добавить пост валидацию
				await SaveChangesAsync();

				if (addresult.Succeeded)
				{
					var entity = dbContext.ExtAttributes.FirstOrDefault(e => e.Id == addresult.Returns);
					if(entity != null)
					attributesValues.Add(entity);
				}
			}
			await SaveChangesAsync();
			return attributesValues;
		}

		public async Task<OperationResult<Guid>> CreateAttributeTypeAsync(ProductAttributeTypeEntity newAttrType)
		{
			if (dbContext.ExtAttributesTypes.Any(e => e.AttributeName == newAttrType.AttributeName))
				return OperationResult<Guid>.Error(localizer["Error.DynAttributeTypes.SimilarNameExists"], HttpStatusCode.BadRequest);
			var res = await dbContext.ExtAttributesTypes.AddAsync(newAttrType);
			return res != null ?
				OperationResult<Guid>.Success(res.Entity.Id) :
				OperationResult<Guid>.Error(localizer["Error.DynAttributeTypes.WriteDbError"], HttpStatusCode.InternalServerError);
		}

		public async Task<OperationResult<Guid>> CreateAttributeValueAsync(ProductAttributeEntity attribute)
		{
			var res = await dbContext.ExtAttributes.AddAsync(attribute);
			return res != null ?
				OperationResult<Guid>.Success(res.Entity.Id) :
				OperationResult<Guid>.Error(localizer["Error.DynAttributeTypes.WriteDbError"], HttpStatusCode.InternalServerError);
		}

		public Task<OperationResult<Guid>> CreateAttributeValue(string attributeType, string atributeValue)
		{
			throw new NotImplementedException();
		}

		public async Task<OperationResult> DeleteAttributeTypeAsync(ProductAttributeTypeEntity newAttrType)
		{
			if (!dbContext.ExtAttributesTypes.Any(e => e.Id == newAttrType.Id))
				return OperationResult.Error(localizer["Error.DynAttributeTypes.DynAttributeTypeNotFound"], HttpStatusCode.NotFound);
			var res = dbContext.ExtAttributesTypes.Remove(newAttrType);
			return res != null ?
				OperationResult.Success() :
				OperationResult.Error(localizer["Error.DynAttributeTypes.DeleteDbError"], HttpStatusCode.InternalServerError);
		}

		public async Task<OperationResult> DeleteAttributeValue(ProductAttributeEntity attributeEntity)
		{
			if (!dbContext.ExtAttributesTypes.Any(e => e.Id == attributeEntity.Id))
				return OperationResult.Error(localizer["Error.DynAttributes.DynAttributeNotFound"], HttpStatusCode.NotFound);
			var res = dbContext.ExtAttributes.Remove(attributeEntity);
			return res != null ?
				OperationResult.Success() :
				OperationResult.Error(localizer["Error.DynAttributes.DeleteDbError"], HttpStatusCode.InternalServerError);
		}

		public async Task<ProductAttributeTypeEntity> GetAttributeTypeEntityAsync(string typeName)
			=> dbContext.ExtAttributesTypes.FirstOrDefault(e => e.AttributeName == typeName);

		public async Task<ProductAttributeTypeEntity> GetAttributeTypeEntityAsync(Guid Id)
			=> dbContext.ExtAttributesTypes.FirstOrDefault(e => e.Id == Id);

		public async Task<ProductAttributeEntity> GetAttributeValue(Guid Id)
			=> dbContext.ExtAttributes.FirstOrDefault(e => e.Id == Id);

		public bool IsAttributeTypeExists(string Name)
			=> dbContext.ExtAttributesTypes.Any(e => e.AttributeName == Name);

		public bool IsAttributeTypeExists(Guid Id)
			=> dbContext.ExtAttributesTypes.Any(e => e.Id == Id);

		public bool IsAttributeValueExists(Guid Id)
			=> dbContext.ExtAttributes.Any(e => e.Id == Id);


		public async Task<OperationResult> RenameAttributeTypeAsync(ProductAttributeTypeEntity newAttrType, string newName)
		{
			if (IsAttributeTypeExists(newName))
				return OperationResult.Error(localizer["Error.DynAttributeTypes.SimilarNameExists"], HttpStatusCode.BadRequest);
			var updentry = dbContext.ExtAttributesTypes.Update(newAttrType);
			if (updentry == null)
				return OperationResult.Error(localizer["Error.DynAttributeTypes.DynAttributeTypeNotFound"], HttpStatusCode.InternalServerError);
			updentry.Entity.AttributeName = newName;
			return OperationResult.Success();
		}
		public async Task<OperationResult> UpdateAttributeValue(ProductAttributeEntity attributeEntity, string newAttributeValue)
		{
			var updentry = dbContext.ExtAttributes.Update(attributeEntity);
			if (updentry == null)
				return OperationResult.Error(localizer["Error.DynAttributeTypes.DynAttributeTypeNotFound"], HttpStatusCode.InternalServerError);
			updentry.Entity.AttributeValue = newAttributeValue;
			return OperationResult.Success();
		}
	}
}
