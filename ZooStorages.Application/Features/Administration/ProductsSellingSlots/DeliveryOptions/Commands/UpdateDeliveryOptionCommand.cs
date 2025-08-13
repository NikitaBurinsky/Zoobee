using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;

namespace ZooStorages.Application.Features.Administration.ProductsSellingSlots.DeliveryOptions.Commands
{
    public class UpdateDeliveryOptionCommand : IRequest<OperationResult>
    {
        public Guid deliveryId { get; set; }
        public TimeSpan DeliveryTime { get; set; }
        public decimal DeliveryCost { get; set; }
        public List<string> PaymentTypes { get; set; }
    }

    public class UpdateDeliveryOptionCommandHandler : IRequestHandler<UpdateDeliveryOptionCommand, OperationResult>
    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;

        public async Task<OperationResult> Handle(UpdateDeliveryOptionCommand request, CancellationToken cancellationToken)
        {
            var delivery = await unitOfWork.ProductSellingSlots.GetDeliveryOptionAsync(request.deliveryId);
            if (delivery == null)
                return OperationResult.Error(_localizer["Error.SlotsDelivery.DeliveryOptionNotFound"], HttpStatusCode.NotFound);
            var res = await unitOfWork.ProductSellingSlots.UpdateDeliveryOptionAsync(delivery, e =>
            {
                e.DeliveryTime = request.DeliveryTime;
                e.DeliveryCost = request.DeliveryCost;
                e.PaymentTypes = request.PaymentTypes;
            });
            return res;
        }
        public UpdateDeliveryOptionCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

    }
}
