using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zoobee.Application.Shared.DTOs.Environment.Manufactures;
using Zoobee.Application.Features.Environment.Manufactures.ProductLineups.Commands;
using Zoobee.Application.Features.Environment.Manufactures.ProductLineups.Queries;

namespace Zoobee.Web.Controllers.Admin
{
	[ApiController]
	[Route("api/admin/product-lineups")]
	public class ProductLineupsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public ProductLineupsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<List<ProductLineupDto>>> GetAll()
		{
			var result = await _mediator.Send(new GetAllProductLineupsQuery());
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create([FromBody] ProductLineupDto dto)
		{
			var id = await _mediator.Send(new CreateProductLineupCommand(dto));
			return CreatedAtAction(nameof(GetAll), new { id }, id);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] ProductLineupDto dto)
		{
			await _mediator.Send(new UpdateProductLineupCommand(dto));
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			await _mediator.Send(new DeleteProductLineupCommand(id));
			return NoContent();
		}
	}
}