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
    public record ListDeliveryOptionsForSlotQuery(Guid slotId) : IRequest<OperationResult<List<DeliveryOptionDto>>>
    { }

    public class ListDeliveryOptionsForSlotQueryHandler : IRequestHandler<ListDeliveryOptionsForSlotQuery, OperationResult<List<DeliveryOptionDto>>>
    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;

        public async Task<OperationResult<List<DeliveryOptionDto>>> Handle(
            ListDeliveryOptionsForSlotQuery request, CancellationToken cancellationToken)
        {
            var slot = await unitOfWork.ProductSellingSlots.GetProductSlotAsync(request.slotId);
            if (slot == null)
                return OperationResult<List<DeliveryOptionDto>>
                    .Error(_localizer["Error.ProductSlots.ProductSlotNotFound"], HttpStatusCode.NotFound);
            await unitOfWork.ProductSellingSlots.LoadDeliveryOptionsAsync(slot);
            var res = slot.DeliveryOptions.Select(DeliveryOptionDto.FromEntity).ToList();
            return OperationResult<List<DeliveryOptionDto>>.Success(res);
        }
        public ListDeliveryOptionsForSlotQueryHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

    }
}
