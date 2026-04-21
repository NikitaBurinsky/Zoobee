using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zoobee.Application.Shared.DTOs.Environment.Pets;
using Zoobee.Application.Features.Environment.Pets.PetKinds.Commands;
using Zoobee.Application.Features.Environment.Pets.PetKinds.Queries;

namespace Zoobee.Web.Controllers.Admin
{
	[ApiController]
	[Route("api/admin/pet-kinds")]
	public class PetKindsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public PetKindsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<List<PetKindDto>>> GetAll()
		{
			var result = await _mediator.Send(new GetAllPetKindsQuery());
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create([FromBody] PetKindDto dto)
		{
			var id = await _mediator.Send(new CreatePetKindCommand(dto));
			return CreatedAtAction(nameof(GetAll), new { id }, id);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] PetKindDto dto)
		{
			await _mediator.Send(new UpdatePetKindCommand(dto));
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			await _mediator.Send(new DeletePetKindCommand(id));
			return NoContent();
		}
	}
}