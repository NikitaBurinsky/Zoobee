using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Domain;

namespace Zoobee.Application.Interfaces.Services.Products.ProductsStorage
{
	/// <summary>
	/// Сервис для записи и обновления в бд информации о продуктах с внешних источников. 
	/// Ключевые задачи сводятся к преобразованию информации о конкретных товарах, и обновлению информации этих товаров с учетом новой информации.\
	/// 
	/// TODO ВАЖНО. ПРИ ДОБАВЛЕНИИ НОВОГО ТИПА ПРОДУКТОВ, ДОБАВИТЬ СООТВЕТСВУЮЩИЙ МЕТОД С НОВЫМ ТИПОМ ПРОДУКТОВ
	/// 
	/// </summary>
	public interface IProductsInfoService
	{
		public OperationResult UpdateAddProductInfo(BaseProductDto dto);
		public OperationResult UpdateAddProductInfo(FoodProductDto dto);
		/// ...
		/// TODO Далее добаляются новые типы продуктов
	}
}
