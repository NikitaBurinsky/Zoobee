using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZooStorages.Application.Features.Administration.Products.Products.Commands.CreateProductCommand;
using ZooStorages.Application.Features.Administration.Products.Products.Queries.GetProductByIdQuery;
using ZooStorages.Application.Features.Administration.Products.Products.Queries.ListProductsQuery;
using ZooStorages.Application.Models.Catalog.Product.Product;

namespace ZooStores.Web.Area.Admin.Products
{
    [ApiController]
	[Route("admin/products")]
	public class ProductsController : Controller
	{
		[HttpPost("add")]
		public async Task<IActionResult> CreateProduct(
			[FromServices] IMediator mediator,
			[FromBody] ProductDto request)  
		{
			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			Console.WriteLine("Request got");
			Console.Clear();
			var result = await mediator.Send(new CreateProductCommand 
				{ newProduct = request});
			if (result.Succeeded)
				return Ok(result.Returns);
			Console.WriteLine(result.Message + result.ErrCode.ToString());
			return result.ToProblemDetails();
		}

		[HttpGet("list")]
		public async Task<IActionResult> ListProducts(
			[FromQuery] ListProductsQuery query,
			[FromServices] IMediator mediator) 
		{
			if(!ModelState.IsValid)
				return BadRequest(ModelState);
			var res = await mediator.Send(query);
			return res.Succeeded ? Ok(res) : res.ToProblemDetails();
		}
		[HttpGet("get-by-id/{productId}")]
		public async Task<IActionResult> GetById(
			Guid productId,
			[FromServices] IMediator mediator)
		{
			var res = await mediator.Send(new GetProductByIdQuery(productId));
			return res.Succeeded ? Ok(res) : res.ToProblemDetails();
		}
	}
}
