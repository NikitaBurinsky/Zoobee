using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;

namespace ZooStorages.Application.Features.Administration.ProductsSellingSlots.DeliveryOptions.Commands
{
    public record DeleteDeliveryOptionCommand(Guid Id) : IRequest<OperationResult>
    { }

    public class DeleteDeliveryOptionCommandHandler : IRequestHandler<DeleteDeliveryOptionCommand, OperationResult>
    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;

        public async Task<OperationResult> Handle(DeleteDeliveryOptionCommand request, CancellationToken cancellationToken)
        {
            var entity = await unitOfWork.ProductSellingSlots.GetDeliveryOptionAsync(request.Id);
            if (entity == null)
                return OperationResult.Error(_localizer["Error.SlotsDelivery.DeliveryOptionNotFound"], HttpStatusCode.NotFound);
            var res = await unitOfWork.ProductSellingSlots.DeleteDeliveryOptionAsync(entity);
            if (res.Succeeded)
                await unitOfWork.SaveChangesAsync();
            return res;
        }
        public DeleteDeliveryOptionCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

    }
}
