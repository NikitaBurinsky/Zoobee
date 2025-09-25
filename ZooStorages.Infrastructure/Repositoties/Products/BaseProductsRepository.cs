using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.Repositoties.Products
{
	public class BaseProductsRepository : IBaseProductsRepository
	{
		ZooStoresDbContext dbContext { get; }
		public BaseProductsRepository(ZooStoresDbContext _dbContext) { 
			dbContext = _dbContext;
		}
		public IQueryable<BaseProductEntity> GetAll() => dbContext.Products;

		public bool IsProductExists(Guid Id) => dbContext.Products.Any(p => p.Id == Id);
	}
}
