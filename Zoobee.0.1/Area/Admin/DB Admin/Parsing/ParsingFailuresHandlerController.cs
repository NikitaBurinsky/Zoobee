using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zoobee.Application.Features.Database_Administration.Parsing.Parsing_Failures_Handling;

namespace Zoobee.Web.Area.Database_Admin.Parsing
{
	[ApiController]
	[Route("db/parsing/failures")]
	[Authorize(Roles = "db-admin,super-admin")]
	public class ParsingFailuresHandlerController : ControllerBase
	{
		[HttpGet("products-info/get-all")]
		public async Task<IActionResult> Index(int page,
			[FromServices] IMediator mediator) 
		{
			var res = await mediator.Send(new GetAllFailedProductInfoTasksQuery { page = page, pageSize = 1 });
			if (res.Succeeded)
				return Ok(res.Returns);
			return BadRequest(res.Message);
		}
	}
}
