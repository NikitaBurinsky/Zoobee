using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Business_Items.Sellings;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Application.Interfaces.Repositories.Sellings;
using Zoobee.Application.Interfaces.Services;
using Zoobee.Application.Interfaces.Services.Products.Catalog;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.Repositoties;

namespace Zoobee.Infrastructure.Services.Products.Catalog
{
	public class SellingSlotsStorageService : ISellingSlotsStorageService
	{
		ISellingSlotsRepository sellingSlotsRepo;
		IBaseProductsRepository productsRepository;
		IMappingService mapper;

		public OperationResult<Guid> SaveSellingSlot(SellingSlotDto dto)
		{
			var res = mapper.Map<SellingSlotDto, SellingSlotEntity>(dto);
			if (res.Failed)
				return OperationResult<Guid>.Error(res);
			var product = productsRepository.Get(dto.ProductId);
			if(product == null)
				return OperationResult<Guid>.Error("")



			throw new NotImplementedException();
		}
	}
}
