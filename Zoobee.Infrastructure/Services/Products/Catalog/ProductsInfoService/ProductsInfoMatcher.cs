using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;

namespace Zoobee.Infrastructure.Services.Products.Catalog.ProductsInfoService
{
	/// <summary>
	/// Класс, ответсвтвенный за сопоставление dto продукта из внешнего источника с
	/// некоторой записью продукта в бд.
	/// </summary>
	public class ProductsInfoMatcher
	{

		IBaseProductsRepository productsRepository;
		public bool TryFindProduct(BaseProductDto dto)
		{
			






		}



	}
}
