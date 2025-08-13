using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZooStorages.Application.Features.Administration.ProductsSellingSlots.DeliveryOptions.Commands;
using ZooStorages.Application.Features.Administration.ProductsSellingSlots.DeliveryOptions.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZooStores.Web.Area.Admin.Products.Selling_Slots
{
	[ApiController]
	[Route("admin/products/selling/delivery-options")]
	public class DeliveryOptionsController : ControllerBase
	{
		[HttpGet("get-by-id/{Id}")]
		public async Task<IActionResult> GetById(
			[FromRoute] Guid Id,
			[FromServices] IMediator mediator)
		{
			var res = await mediator.Send(new GetDeliveryOptionByIdQuery(Id));
			return res.Succeeded ? Ok(res) : res.ToProblemDetails();
		}

		[HttpPost("add")]
		public async Task<IActionResult> Create(
			[FromBody] CreateDeliveryOptionCommand request,
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
			var res = await mediator.Send(new DeleteDeliveryOptionCommand(Id));
			return res.Succeeded ? Ok() : res.ToProblemDetails();
			
		}

		[HttpGet("list-for-slot")]
		public async Task<IActionResult> ListForSlot(
			[FromRoute] Guid slotId,
			[FromServices] IMediator mediator)
		{
			var res = await mediator.Send(new ListDeliveryOptionsForSlotQuery(slotId));
			return res.Succeeded ? Ok(res) : res.ToProblemDetails();
		}

		[HttpPost("update")]
		public async Task<IActionResult> UpdateDelivery(
			[FromBody] UpdateDeliveryOptionCommand request,
			[FromServices] IMediator mediator)
		{
			var res = await mediator.Send(request);
			return res.Succeeded ? Ok() : res.ToProblemDetails();
		}

	}
}
