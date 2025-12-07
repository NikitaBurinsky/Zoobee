using Zoobee.Application.Interfaces.Repositories.IRepositoryBase;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Application.Interfaces.Repositories.UnitsOfWork
{
	public interface IProductsUnitOfWork
	{
		IBaseProductsRepository AllProducts { get; }
		IFoodProductsRepository FoodProductsRepository { get; }
		IToiletProductsRepository ToiletProductsRepository { get; }
		IRepositoryBase<ProductEntity> RepositoryOfType<ProductEntity>() where ProductEntity : BaseProductEntity;
	}
}
