using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZooStorages.Application.Features.Catalog_Features.Categories.Commands;
using ZooStorages.Application.Features.Catalog_Features.Categories.Queries;
using ZooStorages.Application.Features.Products.Categories.Commands;
using ZooStores.Application.DtoTypes.Products.Categories;

namespace ZooStores.Web.Area.Admin.Products
{
    [ApiController]
	[Route("admin/product-categories")]
	public class ProductCategoriesController : Controller
	{
		[HttpPost("add")]
		public async Task<IActionResult> CreateCategory(
			[FromServices] IMediator mediator,
			[FromBody] CreateProductCategoryCommand request)
		{
			var res = await mediator.Send(request);
			return res.Succeeded ? Ok(res) : res.ToProblemDetails();
		}

		[HttpGet("list")]
		public async Task<IActionResult> ListCategories(
			[FromServices] IMediator mediator)
		{
			var res = await mediator.Send(new ListCategoriesQuery());
			return res != null ? Ok(res) : NotFound();
		}

		[HttpDelete("delete")]
		[EndpointDescription("Позволяет удалить только категорию," +
			" не содержащую никаких типов, а следовательно и продуктов")]
		public async Task<IActionResult> DeleteCategory(
			[FromServices] IMediator mediator)
		{
			var res = await mediator.Send(new DeleteProductCategoryCommand());
			return res.Succeeded ? Ok() : res.ToProblemDetails();
		}

		[HttpPost("update")]		
		public async Task<IActionResult> UpdateCategory(
			[FromServices] IMediator mediator,
			[FromBody] UpdateProductCategoryCommand request)
		{
			var res = await mediator.Send(request);
			return res.Succeeded ? Ok() : res.ToProblemDetails();
		} 


	}
}
