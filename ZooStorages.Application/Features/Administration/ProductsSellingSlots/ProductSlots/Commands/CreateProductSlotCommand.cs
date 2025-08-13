using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;
using ZooStores.Application.DtoTypes.Products.Delivery;

namespace ZooStorages.Application.Features.Administration.ProductsSellingSlots.ProductSlots.Commands
{
    public class CreateProductSlotCommand : IRequest<OperationResult<Guid>>
    {
        public Guid ProductId { get; set; }
        public List<Guid> SelfPickupOptions { get; set; }
        public List<Guid> DeliveryOptions { get; set; }
        [Range(0, int.MaxValue)]
        public decimal DefaultSlotPrice { get; set; }
        [Range(0, 100)]
        public decimal Discount { get; set; }
        public bool IsAvaibable { get; set; }
    }

    public class CreateProductSlotCommandHandler : IRequestHandler<CreateProductSlotCommand, OperationResult<Guid>>
    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;

        public async Task<OperationResult<Guid>> Handle(CreateProductSlotCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Products.GetProductAsync(request.ProductId);
            if (product == null)
                return OperationResult<Guid>.Error(_localizer["Error.ProductSlots.ProductNotFound"], HttpStatusCode.NotFound);
            List<DeliveryOptionEntity> deliveries = await GetDeliveryOptionEntities(request.DeliveryOptions);
            List<SelfPickupOptionEntity> selfPickups = await GetSelfPickupOptionEntities(request.SelfPickupOptions);

            ProductSlotEntity newEntity = new ProductSlotEntity
            {
                DefaultSlotPrice = request.DefaultSlotPrice,
                Discount = request.Discount,
                IsAvaibable = request.IsAvaibable,
                Product = product,
                DeliveryOptions = deliveries,
                SelfPickupOptions = selfPickups
            };
            var res = await unitOfWork.ProductSellingSlots.CreateNewProductSlotAsync(newEntity);
            if (res.Succeeded)
                await unitOfWork.SaveChangesAsync();
            return res;
        }
        public CreateProductSlotCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        private async Task<List<DeliveryOptionEntity>> GetDeliveryOptionEntities(List<Guid> Ids)
        {
            List<DeliveryOptionEntity> res = new List<DeliveryOptionEntity>();
            foreach (var i in Ids)
            {
                var entity = await unitOfWork.ProductSellingSlots.GetDeliveryOptionAsync(i);
                if (entity == null) continue; //TODO Возможно пересмотреть. Сейчас невалидные он пропускает
                res.Add(entity);
            }
            return res;
        }
        private async Task<List<SelfPickupOptionEntity>> GetSelfPickupOptionEntities(List<Guid> Ids)
        {
            List<SelfPickupOptionEntity> res = new List<SelfPickupOptionEntity>();
            foreach (var i in Ids)
            {
                var entity = await unitOfWork.ProductSellingSlots.GetSelfPickupOptionAsync(i);
                if (entity == null) continue; //TODO Возможно пересмотреть. Сейчас невалидные он пропускает
                res.Add(entity);
            }
            return res;
        }
    }
}
