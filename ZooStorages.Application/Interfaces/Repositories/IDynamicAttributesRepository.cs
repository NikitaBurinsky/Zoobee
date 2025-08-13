using ZooStorages.Core;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStorages.Domain.DataEntities.Products.Components.DynamicAttributes;
using ZooStorages.Domain.DataEntities.Products.Components.ExtendedAttributes;

namespace ZooStorages.Application.Interfaces.Repositories
{
	public interface IDynamicAttributesRepository : IRepositoryBase
	{
		public IQueryable<ProductAttributeEntity> AttributesValues { get; }
		public IQueryable<ProductAttributeTypeEntity> AttributeTypes { get; }
		//Attribute types
		public Task<OperationResult<Guid>> CreateAttributeTypeAsync(ProductAttributeTypeEntity newAttrType);
		public Task<OperationResult> RenameAttributeTypeAsync(ProductAttributeTypeEntity newAttrType, string newName);
		public Task<OperationResult> DeleteAttributeTypeAsync(ProductAttributeTypeEntity newAttrType);
		public Task<ProductAttributeTypeEntity> GetAttributeTypeEntityAsync(string typeName);
		public Task<ProductAttributeTypeEntity> GetAttributeTypeEntityAsync(Guid Id);
		//Attribute values 
		public Task<OperationResult<Guid>> CreateAttributeValueAsync(ProductAttributeEntity attribute);
		public Task<OperationResult<Guid>> CreateAttributeValue(string attributeType, string atributeValue);
		public Task<ProductAttributeEntity> GetAttributeValue(Guid Id);
		public Task<OperationResult> UpdateAttributeValue(ProductAttributeEntity attributeEntity, string newAttributeValue);
		public Task<OperationResult> DeleteAttributeValue(ProductAttributeEntity attributeEntity);
		public bool IsAttributeTypeExists(string Name);
		public bool IsAttributeTypeExists(Guid Id);
		public bool IsAttributeValueExists(Guid Id);
		public Task<ICollection<ProductAttributeEntity>> CreateAttributesValuesAsync(Dictionary<string, string> attributes);
	}
}





