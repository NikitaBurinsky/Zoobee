using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Application.Interfaces.Repositories.Products_Repositories
{
	public interface IBaseProductsRepository
	{
		public IQueryable<BaseProductEntity> GetAll();
		public bool IsProductExists(Guid Id);
	}
}
