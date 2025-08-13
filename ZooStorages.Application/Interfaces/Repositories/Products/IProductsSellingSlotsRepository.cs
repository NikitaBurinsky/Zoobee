using ZooStorages.Core;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStorages.Domain.DataEntities.Products;
using ZooStores.Application.DtoTypes.Products.Delivery;

namespace ZooStorages.Application.Interfaces.Repositories.Products {
	public interface IProductSellingSlotsRepository : IRepositoryBase
    {
		IQueryable<ProductSlotEntity> ProductSellingSlot { get; }
		
		//Create
		public Task<OperationResult<Guid>> CreateNewProductSlotAsync(ProductSlotEntity newSlot);
		public Task<OperationResult<Guid>> CreateNewSlotDeliveryOptionAsync(DeliveryOptionEntity newDelivery);
		public Task<OperationResult<Guid>> CreateNewSlotSelfPickupOptionAsync(SelfPickupOptionEntity newSelfPickupOption);
        
		//Read
		public Task<ProductSlotEntity> GetProductSlotAsync(Guid Id);
		public Task<DeliveryOptionEntity> GetDeliveryOptionAsync(Guid Id); 
		public Task<SelfPickupOptionEntity> GetSelfPickupOptionAsync(Guid Id);
		public Task LoadDeliveryOptionsAsync(ProductSlotEntity slot);
		public Task LoadSelfPickupOptionsAsync(ProductSlotEntity slot);

		//Check
		public bool IsSlotExists(Guid Id);
		public bool IsDeliveryOptionExists(Guid Id);
		public bool IsSelfPickupOptionExists(Guid Id);

		//Update
		public Task<OperationResult> UpdateSellingSlotAsync(ProductSlotEntity updatedProduct, Action<ProductSlotEntity> action);
		public Task<OperationResult> UpdateSelfPickupOptionAsync(SelfPickupOptionEntity updatedProduct, Action<SelfPickupOptionEntity> action);
		public Task<OperationResult> UpdateDeliveryOptionAsync(DeliveryOptionEntity updatedDeliveryOption, Action<DeliveryOptionEntity> action);

		//Delete
		public Task<OperationResult> DeleteDeliveryOptionAsync(DeliveryOptionEntity deliveryToDelete);
		public Task<OperationResult> DeleteSelfPickupOptionAsync(SelfPickupOptionEntity selfPickupToDelete);

		/// <summary>
		/// Удаляет слот продукта вместе со всеми зависимыми сущностями (списки доставки и самовывоза)
		/// </summary>
		public Task<OperationResult> DeleteProductSlotAbsoluteAsync(ProductSlotEntity newProductSlot);

	}
}
