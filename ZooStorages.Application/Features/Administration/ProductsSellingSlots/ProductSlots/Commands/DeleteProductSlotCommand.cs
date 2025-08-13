using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;

namespace ZooStorages.Application.Features.Administration.ProductsSellingSlots.ProductSlots.Commands
{
    public record DeleteProductSlotCommand(Guid Id) : IRequest<OperationResult>
    { }

    public class DeleteProductSlotCommandHandler : IRequestHandler<DeleteProductSlotCommand, OperationResult>
    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;

        public async Task<OperationResult> Handle(DeleteProductSlotCommand request, CancellationToken cancellationToken)
        {
            ProductSlotEntity slot = await unitOfWork.ProductSellingSlots.GetProductSlotAsync(request.Id);
            if (slot == null)
                return OperationResult.Error(_localizer["Error.ProductSlots.ProductSlotNotFound"], HttpStatusCode.NotFound);
            OperationResult res = await unitOfWork.ProductSellingSlots.DeleteProductSlotAbsoluteAsync(slot);
            if (res.Succeeded)
                await unitOfWork.SaveChangesAsync();
            return res;
        }
        public DeleteProductSlotCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

    }
}
