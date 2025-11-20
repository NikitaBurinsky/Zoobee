using Zoobee.Domain;
using Zoobee.Infrastructure.Parsers.Interfaces.Repositories.IParsedProductsRepository;
using Zoobee.Infrastructure.Parsers.Interfaces.Repositories.IParsersDbContext;
using Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsedDataEntities.ParsedDataIdentifiers;
using Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsedDataEntities.ParsedProductHolder;
using Zoobee.Infrastructure.Parsers.Storage.ParsedDataEntities.ParsedProductsModels.Zoobazar;

namespace Zoobee.Infrastructure.Parsers.ParsedDataStorage.Repositories.Zoobazar
{
	public class ZoobazarParsedProductsRepository : IParsedProductsRepository<ZoobazarParsedProduct>
	{
		IParsersDbContext db;
		public ZoobazarParsedProductsRepository(IParsersDbContext db)
		{
			this.db = db;
		}

		public async Task<OperationResult<Guid>> SaveParsedProduct(ZoobazarParsedProduct product, string parsedUrl, bool SaveChanges = true)
		{
			await db.ZoobazarOffers.AddRangeAsync(product.Offers);
			await db.ZoobazarTabs.AddRangeAsync(product.Tabs);
			var resProduct = await db.ZoobazarParsedProducts.AddAsync(product);
			if (resProduct == null)
			{
				return OperationResult<Guid>.Error("Parsers.Database.Zoobazar.ParsedProduct.WriteDbError", 
					System.Net.HttpStatusCode.InternalServerError);
			}
			else
			{
				var entity = new ParsedProductHolder<ZoobazarParsedProduct>()
				{
					parsedProductModel = resProduct.Entity,
					LastTransformed = null,
					ParsedUrl = parsedUrl
				};
				var res = await db.Zoobazar_ParsedProducts.AddAsync(entity);
				var uptres = await UpdateIdentifiersTable(res.Entity.Id, res.Entity.parsedProductModel.ProductName, parsedUrl);
				await db.SaveChangesAsync();
				return uptres.Succeeded ?
					OperationResult<Guid>.Success(res.Entity.Id) :
					OperationResult<Guid>.Error(uptres);
			}
		}
		protected async Task<OperationResult> UpdateIdentifiersTable(Guid newId, string productName, string parsedUrl)
		{
			var identifier = db.ParsedProductsIdentifiers.FirstOrDefault(e => e.ParsedName == productName && e.ParsedUrl == parsedUrl);
			if (identifier != null)
			{
				db.ParsedProductsIdentifiers.Update(identifier);
				identifier.ParsedDatasOfProduct.Add(newId);
				return OperationResult.Success();
			}
			else
			{
				var newIdentifier = new ParsedProductIdentifierEntity()
				{
					ParsedName = productName,
					ParsedUrl = parsedUrl,
					ParsedDatasOfProduct = new List<Guid> { newId },
				};
				var res = await db.ParsedProductsIdentifiers.AddAsync(newIdentifier);
				if (res == null)
					return OperationResult.Error("Parsers.Database.ParsedProductIdentifiers.WriteDbError", System.Net.HttpStatusCode.InternalServerError);
				return OperationResult.Success();
			}
		}

		public ZoobazarParsedProduct GetParsedProduct(Guid id) =>
			db.Zoobazar_ParsedProducts.FirstOrDefault(e => e.Id == id).parsedProductModel;

		public string GetParsedProductUrl(Guid productId) =>
			db.ParsedProductsIdentifiers.FirstOrDefault(e => e.ParsedDatasOfProduct.Contains(productId)).ParsedUrl;

		public IQueryable<ParsedProductHolder<ZoobazarParsedProduct>> GetAll() => db.Zoobazar_ParsedProducts;

		public async Task<bool> MarkAsTransformedNow(ParsedProductHolder<ZoobazarParsedProduct> parsedProduct)
		{
			var entry = db.Zoobazar_ParsedProducts.Update(parsedProduct);
			if (entry == null)
				return false;
			parsedProduct.LastTransformed = DateTime.Now;
			return true;
		}

		public async Task<int> SaveChangesAsync() => await db.SaveChangesAsync();
	}
}
