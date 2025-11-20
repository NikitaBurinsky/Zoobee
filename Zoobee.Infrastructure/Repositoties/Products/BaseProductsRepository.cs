using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.Repositoties.Products
{
	public class BaseProductsRepository : IBaseProductsRepository
	{
		ZoobeeAppDbContext dbContext { get; }
		public BaseProductsRepository(ZoobeeAppDbContext _dbContext)
		{
			dbContext = _dbContext;
		}


		public IQueryable<BaseProductEntity> GetAll() => dbContext.Products;

		public bool IsProductExists(Guid Id) => dbContext.Products.Any(p => p.Id == Id);
	}
}
