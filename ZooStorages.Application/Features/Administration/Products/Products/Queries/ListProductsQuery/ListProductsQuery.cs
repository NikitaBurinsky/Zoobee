using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core.Errors;
using ZooStorages.Application.IQueriableExtensions;
using Microsoft.EntityFrameworkCore;
using ZooStorages.Application.Models.Catalog.Product.Product;
using ZooStorages.Application.Interfaces.Services.ProductsAdminitrator;
using ZooStorages.Core;

namespace ZooStorages.Application.Features.Administration.Products.Products.Queries.ListProductsQuery
{
    public record ListProductsQuery(int pageSize, int pageNum) : IRequest<OperationResult<List<ProductDto>>>;
    public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, OperationResult<List<ProductDto>>>
    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;
        IProductsAdministratorService productAdministratorService;

        public async Task<OperationResult<List<ProductDto>>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
        {
            var res = productAdministratorService.ListProductsUnordered(request.pageNum, request.pageSize);
            return res;
        }
        public ListProductsQueryHandler(IStringLocalizer<Errors> localizer,
            IMapper mapper, IUnitOfWork unitOfWork,
            IProductsAdministratorService products)
        {
            productAdministratorService = products;
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

    }
}
