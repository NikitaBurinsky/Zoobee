using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Business_Items.Sellings;
using Zoobee.Domain;

namespace Zoobee.Application.Interfaces.Services.Products.Catalog
{
	/// <summary>
	/// Сервис для записи и обновления в бд информации о офферах с внешних источников. 
	/// Ключевые задачи сводятся к преобразованию информации о слотах продажи.
	/// </summary>
	public interface ISellingSlotsInfoService
	{
		/// <summary>
		/// Сохранение нового слота продажи с внешнего источника
		/// Ищет продукт сперва по ProductId, иначе, по SellingUrl
		/// </summary>
		public OperationResult<Guid> MatchAndSaveSellingSlot(SellingSlotDto dto);

		public OperationResult<Guid> SaveSellingSlot(Guid productId, SellingSlotDto dto);

	}
}
