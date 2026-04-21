using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zoobee.Application.Shared.DTOs.Environment.Manufactures;
using Zoobee.Application.Features.Environment.Manufactures.Brands.Commands;
using Zoobee.Application.Features.Environment.Manufactures.Brands.Queries;

namespace Zoobee.Web.Controllers.Admin
{
	[ApiController]
	[Route("api/admin/brands")]
	public class BrandsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public BrandsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<List<BrandDto>>> GetAll()
		{
			var result = await _mediator.Send(new GetAllBrandsQuery());
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create([FromBody] BrandDto dto)
		{
			var id = await _mediator.Send(new CreateBrandCommand(dto));
			return CreatedAtAction(nameof(GetAll), new { id }, id);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] BrandDto dto)
		{
			await _mediator.Send(new UpdateBrandCommand(dto));
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			await _mediator.Send(new DeleteBrandCommand(id));
			return NoContent();
		}
	}
}