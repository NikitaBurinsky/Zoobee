using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products.Components;

namespace ZooStorages.Application.Features.Administration.Products.Products.Queries.GetProductByIdQuery
{
    public record GetProductByIdQuery(Guid Id) : IRequest<OperationResult<ProductEntity>>;

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, OperationResult<ProductEntity>>
    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;

        public async Task<OperationResult<ProductEntity>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await unitOfWork.Products.GetProductAsync(request.Id);
            if (entity == null)
                return OperationResult<ProductEntity>.Error(_localizer["Error.Products.ProductNotFound"], HttpStatusCode.NotFound);
            var response = entity;
            return OperationResult<ProductEntity>.Success(entity);
        }
        public GetProductByIdQueryHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

    }
}
