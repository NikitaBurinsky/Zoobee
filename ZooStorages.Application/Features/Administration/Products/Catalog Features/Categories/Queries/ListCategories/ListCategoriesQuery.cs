using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Application.Models.Catalog.Product.Categories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStores.Application.DtoTypes.Products.Categories;

namespace ZooStorages.Application.Features.Catalog_Features.Categories.Queries
{
    public class ListCategoriesQuery : IRequest<List<ProductCategoryDto>>
	{}

	public class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, List<ProductCategoryDto>>
	{
		IStringLocalizer<Errors> _localizer;
		IMapper _mapper;
		IUnitOfWork unitOfWork;

		public async Task<List<ProductCategoryDto>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
		{
			return unitOfWork.ProductTypes.ProductCategories.Select(e => ProductCategoryDto.FromEntity(e)).ToList();
		}
		public ListCategoriesQueryHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_localizer = localizer;
			_mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

	}
}
