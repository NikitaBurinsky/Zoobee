using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zoobee.Application.Shared.DTOs.Environment.Manufactures;
using Zoobee.Application.Features.Environment.Manufactures.SellerCompanies.Commands;
using Zoobee.Application.Features.Environment.Manufactures.SellerCompanies.Queries;

namespace Zoobee.Web.Controllers.Admin
{
	[ApiController]
	[Route("api/admin/seller-companies")]
	public class SellerCompaniesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public SellerCompaniesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<List<SellerCompanyDto>>> GetAll()
		{
			var result = await _mediator.Send(new GetAllSellerCompaniesQuery());
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create([FromBody] SellerCompanyDto dto)
		{
			var id = await _mediator.Send(new CreateSellerCompanyCommand(dto));
			return CreatedAtAction(nameof(GetAll), new { id }, id);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] SellerCompanyDto dto)
		{
			await _mediator.Send(new UpdateSellerCompanyCommand(dto));
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			await _mediator.Send(new DeleteSellerCompanyCommand(id));
			return NoContent();
		}
	}
}