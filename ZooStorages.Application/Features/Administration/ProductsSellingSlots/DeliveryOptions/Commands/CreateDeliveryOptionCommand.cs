using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using NetTopologySuite.Geometries;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;
using ZooStores.Application.DtoTypes.Products.Delivery;

namespace ZooStorages.Application.Features.Administration.ProductsSellingSlots.DeliveryOptions.Commands
{
    public class CreateDeliveryOptionCommand : IRequest<OperationResult<Guid>>
    {
        public TimeSpan DeliveryTime { get; set; }
        public decimal DeliveryCost { get; set; }
        public Polygon? Area { get; set; }//TODO Сервис а infrastructure с нормальным добавлением области, по гео
        public List<string> PaymentTypes { get; set; }
    }

    public class CreateDeliveryOptionCommandHandler : IRequestHandler<CreateDeliveryOptionCommand, OperationResult<Guid>>
    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;

        public async Task<OperationResult<Guid>> Handle(CreateDeliveryOptionCommand request, CancellationToken cancellationToken)
        {
            DeliveryOptionEntity entity = new DeliveryOptionEntity
            {
                DeliveryTime = request.DeliveryTime,
                DeliveryCost = request.DeliveryCost,
                Area = request.Area,
                PaymentTypes = request.PaymentTypes,
            };
            var res = await unitOfWork.ProductSellingSlots.CreateNewSlotDeliveryOptionAsync(entity);
            if (res.Succeeded)
                await unitOfWork.SaveChangesAsync();
            return res;
        }
        public CreateDeliveryOptionCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

    }
}
