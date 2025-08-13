using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Application.Models.Catalog.Selling.SellingSlots;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using Result = ZooStorages.Core.OperationResult<ZooStorages.Application.Models.Catalog.Selling.SellingSlots.ProductSlotDto>;

namespace ZooStorages.Application.Features.Administration.ProductsSellingSlots.ProductSlots.Queries
{
    public record GetProductSlotByIdQuery(Guid Id) : IRequest<Result>
    { }

    public class GetProductSlotByIdQueryHandler : IRequestHandler<GetProductSlotByIdQuery, Result>
    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;

        public async Task<Result> Handle(GetProductSlotByIdQuery request, CancellationToken cancellationToken)
        {
            var slot = await unitOfWork.ProductSellingSlots.GetProductSlotAsync(request.Id);
            if (slot == null)
                return Result.Error(_localizer["Error.ProductSlots.ProductSlotNotFound"], HttpStatusCode.NotFound);
            return Result.Success(ProductSlotDto.FromEntity(slot));
        }
        public GetProductSlotByIdQueryHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

    }
}
