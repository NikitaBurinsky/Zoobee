using Microsoft.Extensions.Localization;
using Zoobee.Application.DTOs.Filters;
using Zoobee.Application.DTOs.Filters.Products.Toilet_Product;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Application.Filtration.Base;
using Zoobee.Application.Filtration.Food_Products;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork;
using Zoobee.Application.Interfaces.Services.MappingService;
using Zoobee.Application.Interfaces.Services.Products.ProductsFinder;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.Specifications.Toilet_Product;

namespace Zoobee.Infrastructure.Services.Products.ProductsFinder
{
	public class ProductsFinderService : IProductsFinderService
	{
		private IProductsUnitOfWork productsUOW;
		private IStringLocalizer<Errors> lcoalizer;
		private IMappingService dtoMapper;

		public ProductsFinderService(IProductsUnitOfWork productsUOW,
			IStringLocalizer<Errors> lcoalizer,
			IMappingService dtoMapping)
		{
			this.dtoMapper = dtoMapping;
			this.productsUOW = productsUOW;
			this.lcoalizer = lcoalizer;
		}
		public OperationResult<List<FoodProductDto>> GetProductsByFilter(FoodProductFilterDto filter, int PageNum = 1, int PageSize = 15)
		{
			var spec = new FoodProductFilterSpecification(filter, true, PageSize, PageNum);
			List<FoodProductDto> dtos = productsUOW.FoodProductsRepository.GetAll()
				.ApplySpecification(spec)
				.Select(entity => dtoMapper.RevMap<FoodProductDto, FoodProductEntity>(entity).Returns)
				.ToList();
			//TODO Подумать над обработкой ошибок по необзодимости
			return OperationResult<List<FoodProductDto>>.Success(dtos);
		}

		public OperationResult<List<ToiletProductDto>> GetProductsByFilter(ToiletProductFilterDto filter, int PageNum = 1, int PageSize = 15)
		{
			var spec = new ToiletProductFilterSpecification(filter, true, PageSize, PageNum);
			List<ToiletProductDto> dtos = productsUOW.ToiletProductsRepository.GetAll()
				.ApplySpecification(spec)
				.Select(entity => dtoMapper.RevMap<ToiletProductDto, ToiletProductEntity>(entity).Returns)
				.ToList();
			return OperationResult<List<ToiletProductDto>>.Success(dtos);
		}
	}
}
