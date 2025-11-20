using Microsoft.EntityFrameworkCore;
using Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsedDataEntities.ParsedDataIdentifiers;
using Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsedDataEntities.ParsedProductHolder;
using Zoobee.Infrastructure.Parsers.Storage.ParsedDataEntities.ParsedProductsModels.Zoobazar;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Repositories.IParsersDbContext
{
	public interface IParsersDbContext
	{
		DbSet<ParsedProductHolder<ZoobazarParsedProduct>> Zoobazar_ParsedProducts { get; }
		DbSet<ParsedProductIdentifierEntity> ParsedProductsIdentifiers { get; }
		DbSet<ZoobazarParsedProduct> ZoobazarParsedProducts { get; }
		DbSet<Offer> ZoobazarOffers { get; }
		DbSet<Tab> ZoobazarTabs { get; }
		public void ClearEntitiesTracking();
		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		public int SaveChanges();
	}
}
