using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZooStorages.Application.Features.Administration.ProductsSellingSlots.ProductSlots.Commands;
using ZooStorages.Application.Features.Administration.ProductsSellingSlots.ProductSlots.Queries;

namespace ZooStores.Web.Area.Admin.Products.Selling_Slots
{
	[ApiController]
	[Route("admin/products/selling/slots")]
	public class ProductsSellingSlotsController : Controller
	{
		[HttpGet("list-for-product")]
		public async Task<IActionResult> List(
			[FromQuery] Guid productId,
			[FromServices] IMediator mediator)
		{
			var res = await mediator.Send(new ListSellingSlotsForProductQuery(productId));
			return res.Succeeded ? Ok(res) : res.ToProblemDetails();
		}

		[HttpGet("get-by-id/{Id}")]
		public async Task<IActionResult> GetById(
			[FromRoute] Guid Id,
			[FromServices] IMediator mediator)
		{
			var res = await mediator.Send(new GetProductSlotByIdQuery(Id));
			return res.Succeeded ? Ok(res) : res.ToProblemDetails();
		}

		[HttpPost("add")]
		public async Task<IActionResult> Add(
			[FromBody] CreateProductSlotCommand request,
			[FromServices] IMediator mediator)
		{
			var res = await mediator.Send(request);
			return res.Succeeded ? Ok(res) : res.ToProblemDetails();
		}

		[HttpDelete("delete/{Id}")]
		public async Task<IActionResult> Delete(
			[FromRoute] Guid Id,
			[FromServices] IMediator mediator)
		{
			var res = await mediator.Send(new DeleteProductSlotCommand(Id));
			return res.Succeeded ? Ok(res) : res.ToProblemDetails();

		}
	}
}
