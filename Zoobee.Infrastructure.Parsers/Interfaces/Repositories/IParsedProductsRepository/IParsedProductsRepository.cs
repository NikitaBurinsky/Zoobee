using Zoobee.Domain;
using Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsedDataEntities.ParsedProductHolder;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Repositories.IParsedProductsRepository
{
	public interface IParsedProductsRepository<ParsedProductType>
		where ParsedProductType : class
	{
		public IQueryable<ParsedProductHolder<ParsedProductType>> GetAll();

		public Task<OperationResult<Guid>> SaveParsedProduct(ParsedProductType product, string parsedUrl, bool SaveChanges = true);
		public ParsedProductType GetParsedProduct(Guid id);
		public string GetParsedProductUrl(Guid productId);
		public Task<bool> MarkAsTransformedNow(ParsedProductHolder<ParsedProductType> type);
		public Task<int> SaveChangesAsync();
	}
}
