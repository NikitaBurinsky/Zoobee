using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Application.Models.Catalog.Selling.SellingSlots;
using ZooStorages.Core;
using ZooStorages.Core.Errors;

namespace ZooStorages.Application.Features.Administration.ProductsSellingSlots.ProductSlots.Queries
{
    public record ListSellingSlotsForProductQuery(Guid productId) : IRequest<OperationResult<List<ProductSlotDto>>>;

    public class ListSellingSlotsForProductQueryHandler : IRequestHandler<ListSellingSlotsForProductQuery, OperationResult<List<ProductSlotDto>>>

    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;

        public async Task<OperationResult<List<ProductSlotDto>>> Handle(ListSellingSlotsForProductQuery request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Products.GetProductAsync(request.productId);
            if (product == null)
                return OperationResult<List<ProductSlotDto>>.Error(_localizer["Error.ProducSlots.ProductNotFound"], HttpStatusCode.NotFound);

            var res = product.SellingSlots.Select(ProductSlotDto.FromEntity).ToList();
            return OperationResult<List<ProductSlotDto>>.Success(res);
        }
        public ListSellingSlotsForProductQueryHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

    }
}
