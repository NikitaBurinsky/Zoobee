using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products.Components;

namespace ZooStorages.Application.Features.Products.Types.Queries
{
	public class ListProductsByTypeQuery : IRequest<OperationResult<List<ProductEntity>>>
	{
		public string TypeName { get; set; }
	}

	public class ListProductsByTypeQueryHandler : IRequestHandler<ListProductsByTypeQuery, OperationResult<List<ProductEntity>>>
	{
		IStringLocalizer<Errors> _localizer;
		IMapper _mapper;
		IUnitOfWork unitOfWork;

		public async Task<OperationResult<List<ProductEntity>>> Handle(ListProductsByTypeQuery request, CancellationToken cancellationToken)
		{
			var type = await unitOfWork.ProductTypes.GetProductTypeAsync(request.TypeName);
			if (type == null)
				return OperationResult<List<ProductEntity>>.Error(_localizer["Error.ProductTypes.ProductTypeNotFound"], HttpStatusCode.NotFound);
			return await unitOfWork.ProductTypes.GetProductsOfTypeAsync(type);
		}
		public ListProductsByTypeQueryHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_localizer = localizer;
			_mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

	}
}
