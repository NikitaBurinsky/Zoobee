using System.Reflection;
using Zoobee.Application.Interfaces.Repositories.IRepositoryBase;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.Repositoties.UnitsOfWork
{
	public class ProductsUnitOfWork : IProductsUnitOfWork
	{
		public ProductsUnitOfWork(IFoodProductsRepository foodProductsRepository,
			IToiletProductsRepository toiletProductsRepository,
			IBaseProductsRepository allProducts)
		{
			FoodProductsRepository = foodProductsRepository;
			ToiletProductsRepository = toiletProductsRepository;
			AllProducts = allProducts;
		}

		public IFoodProductsRepository FoodProductsRepository { get; }
		public IToiletProductsRepository ToiletProductsRepository { get; }
		public IBaseProductsRepository AllProducts { get; }

		public IRepositoryBase<ProductEntity> RepositoryOfType<ProductEntity>() where ProductEntity : BaseProductEntity
		{
			var property = this.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.FirstOrDefault(prop => IsRepositoryForEntity<ProductEntity>(prop.PropertyType));

			if (property == null)
				throw new InvalidOperationException($"Repository for entity {typeof(ProductEntity).Name} not found in IProductsUnitOfWork");

			var repository = property.GetValue(this) as IRepositoryBase<ProductEntity>;

			if (repository == null)
				throw new InvalidOperationException($"Property {property.Name} is not of type IRepositoryBase<{typeof(ProductEntity).Name}>");

			return repository;
		}
		private static bool IsRepositoryForEntity<TEntity>(Type repositoryType)
		{
			return repositoryType.GetInterfaces()
				.Any(i => i.IsGenericType &&
						 i.GetGenericTypeDefinition() == typeof(IRepositoryBase<>) &&
						 i.GetGenericArguments()[0] == typeof(TEntity));
		}
	}
}
