using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using ZooStorages.Application.Features.Products.Catalog_Features.Types.Queries.ListAllTypes;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;

namespace ZooStorages.Application.Features.Catalog_Features.Types.Queries
{
	public class ListAllProductTypesQuery : IRequest<List<ListAllProductTypesDto>>
	{
	}

	public class ListAllProductTypesQueryHandler : IRequestHandler<ListAllProductTypesQuery, List<ListAllProductTypesDto>>
	{
		IStringLocalizer<Errors> _localizer;
		IMapper _mapper;
		IUnitOfWork unitOfWork;

		public async Task<List<ListAllProductTypesDto>> Handle(ListAllProductTypesQuery request, CancellationToken cancellationToken)
		{
			return unitOfWork.ProductTypes.ProductTypes.Select(e => new ListAllProductTypesDto
			{
				Name = e.Name,
				Category = e.Category.CategoryName,
				Information = e.Information,
			}).ToList();
		}
		public ListAllProductTypesQueryHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_localizer = localizer;
			_mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

	}
}
