using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ZooStorages.Application.Features.Catalog_Features.PetKinds;
using ZooStorages.Application.Features.Catalog_Features.PetKinds.Command;
using ZooStorages.Application.Features.Catalog_Features.PetKinds.Queries;
using ZooStorages.Core.Errors;

namespace ZooStores.Web.Area.Admin.Products
{
	[ApiController]
	[Route("admin/pet-kinds")]
	public class PetKindsController : Controller
	{
		public IStringLocalizer<Errors> localizer;

		public PetKindsController(IStringLocalizer<Errors> localizer)
		{
			this.localizer = localizer;
		}

		[HttpPost("add")]
		public async Task<IActionResult> AddPetKind(
			[FromServices] IMediator mediator,
			CreatePetKindCommand request)
		{
			var res = await mediator.Send(request);
			return res.Succeeded ?
				Ok(res) : res.ToProblemDetails();
		}

		[HttpGet("list")]
		public async Task<IActionResult> ListPetKinds(
			[FromServices] IMediator mediator)
		{
			var request = new ListPetKindsQuery();
			var res = await mediator.Send(request);
			return res != null ?
				Ok(res) : NotFound(localizer["Error.PetKinds.PetKindNotFound"]);
		}

		[HttpDelete("delete")]
		public async Task<IActionResult> DeletePetKind(
			[FromServices] IMediator mediator,
			[FromBody] DeletePetKindCommand request) 
		{
			var res = await mediator.Send(request);
			return res.Succeeded ?
				Ok(res) : res.ToProblemDetails();
		}

		[HttpPost("rename-pt")]
		public async Task<IActionResult> UpdatePetKind(
			[FromServices] IMediator mediator,
			[FromBody] RenamePetKindCommand request)
		{
			var res = await mediator.Send(request);
			return res.Succeeded ?
				Ok(res) : res.ToProblemDetails();
		}
	}
}
