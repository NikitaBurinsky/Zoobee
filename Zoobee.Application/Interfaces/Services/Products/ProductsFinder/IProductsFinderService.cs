using Zoobee.Application.DTOs.Filters;
using Zoobee.Application.DTOs.Filters.Products.Toilet_Product;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Domain;

namespace Zoobee.Application.Interfaces.Services.Products.ProductsFinder
{
	/// <summary>
	/// Сервис, отделяющий и концентрирующий логику crud всех типов продуктов
	/// </summary>
	public interface IProductsFinderService
	{
		public OperationResult<List<FoodProductDto>> GetProductsByFilter(FoodProductFilterDto filter, int PageNum = 1, int PageSize = 15);
		public OperationResult<List<ToiletProductDto>> GetProductsByFilter(ToiletProductFilterDto filter, int PageNum = 1, int PageSize = 15);
	}
}
