using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Business_Items.Base;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain.DataEntities.Environment.Manufactures;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Domain.DataEntities.SellingsInformation;

namespace Zoobee.Application.DTOs.Business_Items.Sellings
{
	public class SellingSlotDto : BaseEntityItemDto
	{
		public Guid Id { get; set; }
		public string SellingUrl { get; set; }
		public string SellerCompanyName { get; set; }
		/// <summary>
		/// Базовая стоимость слота, без учета скидок
		/// </summary>
		public decimal DefaultSlotPrice { get; set; }
		public decimal Discount { get; set; }
		/// <summary>
		/// Итоговая стоимость, с учетом скидки 
		/// </summary>
		public decimal ResultPrice { get; set; }
		public Guid ProductId { get; set; }
		/* TODO В будущем добавить и эти поля из парсеров
		public ICollection<DeliveryOptionEntity> DeliveryOptions { get; set; }
		public ICollection<SelfPickupOptionEntity> SelfPickupOptions { get; set; }
		*/
	}
}
