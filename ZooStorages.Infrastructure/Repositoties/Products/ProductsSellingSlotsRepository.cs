using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStores.Application.DtoTypes.Products.Delivery;
using ZooStores.Infrastructure.Repositoties;

namespace ZooStorages.Infrastructure.Repositoties.Products
{
	public class ProductsSellingSlotsRepository : RepositoryBase, IProductSellingSlotsRepository
	{
		public ProductsSellingSlotsRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<ProductSlotEntity> ProductSellingSlot => dbContext.SellingSlots;

		public async Task<OperationResult<Guid>> CreateNewProductSlotAsync(ProductSlotEntity newSlot) {
			if (!dbContext.Products.Any(e => e.Id == newSlot.Product.Id))
				return OperationResult<Guid>.Error(localizer["Error.ProducSlots.ProductNotFound"], HttpStatusCode.BadRequest);
			var entry = await dbContext.SellingSlots.AddAsync(newSlot);
			return entry != null ?
				OperationResult<Guid>.Success(entry.Entity.Id) :
				OperationResult<Guid>.Error(localizer["Error.ProducSlots.WriteDbFailure"], HttpStatusCode.InternalServerError);
		}

		public async Task<OperationResult<Guid>> CreateNewSlotDeliveryOptionAsync(DeliveryOptionEntity newDelivery) {
			var entry = await dbContext.DeliveryOptions.AddAsync(newDelivery);
			return entry != null ?
				OperationResult<Guid>.Success(entry.Entity.Id) :
				OperationResult<Guid>.Error(localizer["Error.ProducSlots.WriteDbFailure"], HttpStatusCode.InternalServerError);
		}

		public async Task<OperationResult<Guid>> CreateNewSlotSelfPickupOptionAsync(SelfPickupOptionEntity newSelfPickupOption) {
			var entry = await dbContext.SelfPickupOptions.AddAsync(newSelfPickupOption);
			return entry != null ?
				OperationResult<Guid>.Success(entry.Entity.Id) :
				OperationResult<Guid>.Error(localizer["Error.ProducSlots.WriteDbFailure"], HttpStatusCode.InternalServerError);
		}

		public async Task<OperationResult> DeleteDeliveryOptionAsync(DeliveryOptionEntity deliveryToDelete) {
			if (IsDeliveryOptionExists(deliveryToDelete.Id))
			{
				var entry = dbContext.Entry(deliveryToDelete);
				if (entry != null)
				{
					dbContext.DeliveryOptions.Remove(deliveryToDelete);
					return OperationResult.Success();
				}
			}
			return OperationResult.Error(localizer["Error.SlotsDelivery.DeliveryOptionNotFound"], HttpStatusCode.NotFound);
		}

		public async Task<OperationResult> DeleteProductSlotAbsoluteAsync(ProductSlotEntity newProductSlot) {
			if (!IsSlotExists(newProductSlot.Id))
				return OperationResult.Error(localizer["Error.ProducSlots.SlotNotFound"], HttpStatusCode.NotFound);

			foreach (var sp in newProductSlot.SelfPickupOptions)
				dbContext.SelfPickupOptions.Remove(sp);
			foreach (var deliv in newProductSlot.DeliveryOptions)
				dbContext.DeliveryOptions.Remove(deliv);
			return OperationResult.Success();
		}

		public async Task<OperationResult> DeleteSelfPickupOptionAsync(SelfPickupOptionEntity selfPickupToDelete) {
			if (IsSelfPickupOptionExists(selfPickupToDelete.Id))
			{
				var entry = dbContext.Entry(selfPickupToDelete);
				if (entry != null)
				{
					dbContext.SelfPickupOptions.Remove(selfPickupToDelete);
					return OperationResult.Success();
				}
			}
			return OperationResult.Error(localizer["Error.SlotsSelfPickup.SelfPickupOptionNotFound"], HttpStatusCode.NotFound);
		}

		public async Task<DeliveryOptionEntity> GetDeliveryOptionAsync(Guid Id)
			=> dbContext.DeliveryOptions.FirstOrDefault(e => e.Id == Id);

		public async Task<ProductSlotEntity> GetProductSlotAsync(Guid Id)
			=> dbContext.SellingSlots.FirstOrDefault(e => e.Id == Id);

		public async Task<SelfPickupOptionEntity> GetSelfPickupOptionAsync(Guid Id)
			=> dbContext.SelfPickupOptions.FirstOrDefault(e => e.Id == Id);

		public bool IsDeliveryOptionExists(Guid Id)
			=> dbContext.DeliveryOptions.Any(e => e.Id == Id);

		public bool IsSelfPickupOptionExists(Guid Id)
			=> dbContext.SelfPickupOptions.Any(e => e.Id == Id);

		public bool IsSlotExists(Guid Id)
			=> dbContext.SellingSlots.Any(e => e.Id == Id);

		public async Task LoadDeliveryOptionsAsync(ProductSlotEntity product)
		{
			var entry = dbContext.Entry(product);
			if (entry == null)
				return;
			await entry.Collection(e => e.DeliveryOptions).LoadAsync();
		}	

		public async Task LoadSelfPickupOptionsAsync(ProductSlotEntity product)
		{
			var entry = dbContext.Entry(product);
			if (entry == null)
				return;
			await entry.Collection(e => e.SelfPickupOptions).LoadAsync();
		}

		public async Task<OperationResult> UpdateDeliveryOptionAsync(DeliveryOptionEntity updatedDeliveryOption, Action<DeliveryOptionEntity> action) {
			var entry = dbContext.Update(updatedDeliveryOption);
			if(entry == null)
				return OperationResult.Error(localizer["Error.SlotsDelivery.DeliveryOptionNotFound"], HttpStatusCode.NotFound);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public async Task<OperationResult> UpdateSelfPickupOptionAsync(SelfPickupOptionEntity updatedSelfPickup, Action<SelfPickupOptionEntity> action) {
			var entry = dbContext.Update(updatedSelfPickup);
			if (entry == null)
				return OperationResult.Error(localizer["Error.SlotsSelfPickup.SelfPickupOptionNotFound"], HttpStatusCode.NotFound);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public async Task<OperationResult> UpdateSellingSlotAsync(ProductSlotEntity updatedProductSlot, Action<ProductSlotEntity> action) {
			var entry = dbContext.Update(updatedProductSlot);
			if (entry == null)
				return OperationResult.Error(localizer["Error.ProducSlots.SellingSlotNotFound"], HttpStatusCode.NotFound);
			action(entry.Entity);
			return OperationResult.Success();
		}
	}
}
