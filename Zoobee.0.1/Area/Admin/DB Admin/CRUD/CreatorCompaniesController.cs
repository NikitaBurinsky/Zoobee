using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zoobee.Application.Shared.DTOs.Environment.Manufactures;
using Zoobee.Application.Features.Environment.Manufactures.CreatorCompanies.Commands;
using Zoobee.Application.Features.Environment.Manufactures.CreatorCompanies.Queries;

namespace Zoobee.Web.Controllers.Admin
{
	[ApiController]
	[Route("api/admin/creator-companies")]
	public class CreatorCompaniesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public CreatorCompaniesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<List<CreatorCompanyDto>>> GetAll()
		{
			var result = await _mediator.Send(new GetAllCreatorCompaniesQuery());
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create([FromBody] CreatorCompanyDto dto)
		{
			var id = await _mediator.Send(new CreateCreatorCompanyCommand(dto));
			return CreatedAtAction(nameof(GetAll), new { id }, id);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] CreatorCompanyDto dto)
		{
			await _mediator.Send(new UpdateCreatorCompanyCommand(dto));
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			await _mediator.Send(new DeleteCreatorCompanyCommand(id));
			return NoContent();
		}
	}
}