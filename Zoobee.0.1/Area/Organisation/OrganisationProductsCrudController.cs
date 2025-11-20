using Microsoft.AspNetCore.Mvc;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;

namespace ZooStores.Web.Area.Organisation
{
	[ApiController]
	[Route("/organisations/products")]
	public class OrganisationProductsCrudController : Controller
	{
		[HttpPost("create")]
		public IActionResult CreateFoodProduct(
			[FromBody] FoodProductDto foodProduct,
			[FromServices] IFoodProductsRepository productsRepository)
		{
			return View(foodProduct);
		}
	}
}
