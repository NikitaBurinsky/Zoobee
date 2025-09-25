using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork;

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
		public IToiletProductsRepository ToiletProductsRepository {  get; }
		public IBaseProductsRepository AllProducts { get; }
	}
}
