using Microsoft.AspNetCore.Mvc;
using Zoobee.Application.DTOs.Filters;
using Zoobee.Application.DTOs.Filters.Products.Toilet_Product;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Application.Interfaces.Services.Products.ProductsFinder;

namespace ZooStores.Web.Area.Customer
{
	[ApiController]
	[Route("catalog")]
	public class ProductsFinderController : ControllerBase
	{
		[HttpGet("toilets")]
		public async Task<IActionResult> FindToilets(
			[FromQuery] ToiletProductFilterDto filter,
			[FromServices] IProductsFinderService finderService)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			List<ToiletProductDto> res = finderService.GetProductsByFilter(filter).Returns;
			return Ok(res);
		}

		[HttpGet("food")]
		public async Task<IActionResult> FindFood(
			[FromQuery] FoodProductFilterDto filter,
			[FromServices] IProductsFinderService finderService)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			List<FoodProductDto> res = finderService.GetProductsByFilter(filter).Returns;
			return Ok(res);
		}

	}
}
