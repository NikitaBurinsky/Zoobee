using Microsoft.EntityFrameworkCore;
using System.Net;
using Zoobee.Application.DTOs.Business_Items.Sellings;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Application.Interfaces.Repositories.Sellings;
using Zoobee.Application.Interfaces.Services.Products.Catalog;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.Services.Products.Catalog.SellingSlotStorageService
{
	public class SellingSlotsStorageService : ISellingSlotsInfoService
	{
		ISellingSlotsRepository sellingSlotsRepo;
		IBaseProductsRepository productsRepository;
		ISellerCompanyRepository sellersRepo;

		public SellingSlotsStorageService(ISellingSlotsRepository sellingSlotsRepo, IBaseProductsRepository productsRepository, ISellerCompanyRepository sellersRepo)
		{
			this.sellingSlotsRepo = sellingSlotsRepo;
			this.productsRepository = productsRepository;
			this.sellersRepo = sellersRepo;
		}

		public OperationResult<Guid> MatchAndSaveSellingSlot(SellingSlotDto dto)
		{
			var entity = new SellingSlotEntity
			{
				DefaultSlotPrice = dto.DefaultSlotPrice,
				Discount = dto.Discount,
				SellingUrl = dto.SellingUrl,
				ResultPrice = dto.ResultPrice,
			};

			//Пробуем найти через ProductId
			var product = productsRepository.Get(dto.ProductId);
			//В ином случае, через SellingUrl
			if (product == null)
			{
				product = sellingSlotsRepo.GetAll()
					.Where(e => e.SellingUrl == dto.SellingUrl).Include(e => e.Product)
					.Select(e => e.Product).FirstOrDefault();
			}	
			if (product == null)
				return OperationResult<Guid>.Error("Error.SellingSlots.ProductNotFound", HttpStatusCode.BadRequest);
			entity.Product = product;

			var seller = sellersRepo.Get(dto.SellerCompanyName);
			if(seller == null)
				return OperationResult<Guid>.Error("Error.SellingSlots.SellerCompanyNotFound", HttpStatusCode.BadRequest);
			entity.SellerCompany = seller;

			var res = sellingSlotsRepo.CreateAsync(entity);
			return res.Result;
		}

		public OperationResult<Guid> SaveSellingSlot(Guid id, SellingSlotDto dto)
		{
			var entity = new SellingSlotEntity
			{
				DefaultSlotPrice = dto.DefaultSlotPrice,
				Discount = dto.Discount,
				SellingUrl = dto.SellingUrl,
				ResultPrice = dto.ResultPrice,
			};
			var product = productsRepository.Get(id);
			if (product == null)
				return OperationResult<Guid>.Error("Error.SellingSlots.ProductNotFound", HttpStatusCode.BadRequest);
			entity.Product = product;
			var seller = sellersRepo.Get(dto.SellerCompanyName);
			if (seller == null)
				return OperationResult<Guid>.Error("Error.SellingSlots.SellerCompanyNotFound", HttpStatusCode.BadRequest);
			entity.SellerCompany = seller;
			var res = sellingSlotsRepo.CreateAsync(entity);
			return res.Result;
		}
	}
}
