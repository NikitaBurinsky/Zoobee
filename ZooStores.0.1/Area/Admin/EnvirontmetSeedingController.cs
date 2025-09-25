using Microsoft.AspNetCore.Mvc;
using Zoobee.Application.Interfaces.Services.EnvirontmentDataSeeding;

namespace ZooStores.Web.Area.Admin
{
	[ApiController]
	[Route("admin/seeding")]
	public class EnvirontmetSeedingController : ControllerBase
	{
		[HttpPost("pet-kinds")]
		public async Task<IActionResult> SeedPetKinds(
			IFormFile petKinds,
			[FromServices] IEnvirontmentDataSeedingService seedingService
			)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var res = await seedingService.JsonSeedPetKinds(petKinds.OpenReadStream());
			return Ok(res);
		}

		[HttpPost("brands")]
		public async Task<IActionResult> SeedBrands(
			IFormFile brands,
			[FromServices] IEnvirontmentDataSeedingService seedingService)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var res = await seedingService.JsonSeedBrands(brands.OpenReadStream());
			return Ok(res);
		}

		[HttpPost("creator-countries")]
		public async Task<IActionResult> SeedCreatorCountries(
			IFormFile creatorCountries,
			[FromServices] IEnvirontmentDataSeedingService seedingService)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var res = await seedingService.JsonSeedCreatorCountries(creatorCountries.OpenReadStream());
			return Ok(res);
		}

		[HttpPost("product-lineups")]
		public async Task<IActionResult> SeedProductLineups(
			IFormFile lineups,
			[FromServices] IEnvirontmentDataSeedingService seedingService)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var res = await seedingService.JsonSeedProductLineups(lineups.OpenReadStream());
			return Ok(res);

		}

		[HttpPost("seller-companies")]
		public async Task<IActionResult> SeedSellerCompanies(
			IFormFile sellerCompanies,
			[FromServices] IEnvirontmentDataSeedingService seedingService)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var res = await seedingService.JsonSeedSellerCompanies(sellerCompanies.OpenReadStream());
			return Ok(res);

		}

		[HttpPost("zoostores")]
		public async Task<IActionResult> SeedZooStores(
			IFormFile creatorCompanies,
			[FromServices] IEnvirontmentDataSeedingService seedingService)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var res = await seedingService.JsonSeedZooStores(creatorCompanies.OpenReadStream());
			return Ok(res);

		}

		[HttpPost("delivery-areas")]
		public async Task<IActionResult> SeedDeliveryAreas(
			IFormFile deliveryAreas,
			[FromServices] IEnvirontmentDataSeedingService seedingService)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var res = await seedingService.JsonSeedDeliveryAreas(deliveryAreas.OpenReadStream());
			return Ok(res);

		}

		[HttpPost("creator-companies")]
		public async Task<IActionResult> SeedCreatorCompanies(
			IFormFile creatorCompanies,
			[FromServices] IEnvirontmentDataSeedingService seedingService)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var res = await seedingService.JsonSeedCreatorCountries(creatorCompanies.OpenReadStream());
			return Ok(res);

		}
	}
}
