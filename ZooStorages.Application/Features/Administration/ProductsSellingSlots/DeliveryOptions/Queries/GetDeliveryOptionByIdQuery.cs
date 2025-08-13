using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Application.Models.Catalog.Selling.DeliveryOptions;
using ZooStorages.Core;
using ZooStorages.Core.Errors;

namespace ZooStorages.Application.Features.Administration.ProductsSellingSlots.DeliveryOptions.Queries
{
    public record GetDeliveryOptionByIdQuery(Guid Id) : IRequest<OperationResult<DeliveryOptionDto>>
    { }

    public class GetDeliveryOptionByIdQueryHandler : IRequestHandler<GetDeliveryOptionByIdQuery, OperationResult<DeliveryOptionDto>>
    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;

        public async Task<OperationResult<DeliveryOptionDto>> Handle(GetDeliveryOptionByIdQuery request, CancellationToken cancellationToken)
        {
            var delivery = await unitOfWork.ProductSellingSlots.GetDeliveryOptionAsync(request.Id);
            if (delivery == null)
                return OperationResult<DeliveryOptionDto>.Error(_localizer["Error.SlotsDelivery.DeliveryOptionNotFound"], HttpStatusCode.NotFound);
            return OperationResult<DeliveryOptionDto>.Success(DeliveryOptionDto.FromEntity(delivery));
        }
        public GetDeliveryOptionByIdQueryHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

    }
}
