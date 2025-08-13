using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ZooStorages.Application.Features.Catalog_Features.Types.Queries;
using ZooStorages.Application.Features.Products.Types.Commands;
using ZooStorages.Application.Features.Products.Types.Queries;

namespace ZooStores.Web.Area.Admin.Products
{
    [ApiController]
	[Route("admin/product-types")]
	public class ProductTypesController : Controller
	{
		[HttpPost("add")]
		public async Task<IActionResult> CreateProductType(
			[FromServices] IMediator mediator,
			[FromBody] CreateProductTypeCommand request) 
		{
			var res = await mediator.Send(request);
			return res.Succeeded ? Ok(res.Returns) : res.ToProblemDetails();
		}

		[HttpGet("list-products-by-type/{typename}")]
		public async Task<IActionResult> ListByType(
			[FromServices] IMediator mediator,
			[FromRoute] string typename)
		{
			var res = await mediator.Send(new ListProductsByTypeQuery { TypeName = typename });
			return res.Succeeded ? Ok(res.Returns) : res.ToProblemDetails();
		}

		[HttpGet("list-types")]
		public async Task<IActionResult> ListTypes(
			[FromServices] IMediator mediator)
		{
			var res = await mediator.Send(new ListAllProductTypesQuery());
			return res != null ? Ok(res) : NotFound();
		}
	}
}
