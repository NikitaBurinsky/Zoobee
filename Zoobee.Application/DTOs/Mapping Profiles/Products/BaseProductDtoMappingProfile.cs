using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.Interfaces.Repositories.Catalog;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment;
using Zoobee.Application.Interfaces.Services.Products.ProductsMapperService.Mapping_Profiles;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Catalog.Tags;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Application.DTOs.Mapping_Profiles.Products
{
	public abstract class BaseProductDtoMappingProfile<ProductDto, ProductEntity> : IBaseMappingProfile<ProductDto, ProductEntity>
		where ProductDto : BaseProductDto
		where ProductEntity : BaseProductEntity
	{
		IStringLocalizer<Errors> localizer;
		IEnvirontmentDataUnitOfWork envUOW { get; }
		ITagsRepository tagsRepository { get; }

		protected BaseProductDtoMappingProfile(IStringLocalizer<Errors> localizer, IEnvirontmentDataUnitOfWork envUOW, ITagsRepository tagsRepository)
		{
			this.localizer = localizer;
			this.envUOW = envUOW;
			this.tagsRepository = tagsRepository;
		}

		/// <summary>
		/// Определяет все поля BaseProductEntity
		/// </summary>
		public abstract OperationResult<ProductEntity> Map(ProductDto from);
		protected OperationResult SetBaseProductFields(BaseProductEntity entity, BaseProductDto from)
		{
			var creatorCountry = envUOW.ManufacturesUOWork.CreatorCountryRepository.Get(from.CreatorCountryName);
			if (creatorCountry == null)
				return OperationResult.Error(localizer["Error.BaseProductDto.Mapping.CreatorCountryNotFound"], HttpStatusCode.InternalServerError);
			var creatorCompany = envUOW.ManufacturesUOWork.CreatorCompaniesRepository.Get(from.CreatorCompanyName);
			if (creatorCompany == null)
				return OperationResult.Error(localizer["Error.BaseProductDto.Mapping.CreatorCompanyNotFound"], HttpStatusCode.InternalServerError);
			var brand = envUOW.ManufacturesUOWork.BrandsRepository.Get(from.BrandName);
			if (brand == null)
				return OperationResult.Error(localizer["Error.BaseProductDto.Mapping.BrandNotFound"], HttpStatusCode.InternalServerError);
			var lineup = envUOW.ManufacturesUOWork.ProductLineupRepository.Get(from.BrandName, from.ProductLineupName);
			if (lineup == null)
				return OperationResult.Error(localizer["Error.BaseProductDto.Mapping.ProductLineupNotFound"], HttpStatusCode.InternalServerError);
			var petKind = envUOW.PetsDataUOWork.petKindsRepository.Get(from.PetKind);
			if (petKind == null)
				return OperationResult.Error(localizer["Error.BaseProductDto.Mapping.PetKindNotFound"], HttpStatusCode.InternalServerError);
			List<TagEntity> tags = tagsRepository.GetAll().Where(e => e.ProductType == typeof(ProductEntity) && from.Tags.Contains(e.TagName)).ToList();
			if (tags == null)
				return OperationResult.Error(localizer["Error.BaseProductDto.Mapping.TagsNotFound"], HttpStatusCode.InternalServerError);

			entity.Name = from.Name;
			entity.Description = from.Description;
			entity.SiteArticles = from.SiteArticles;
			entity.UPC = from.UPC;
			entity.EAN = from.EAN;
			entity.AverageRating = from.AverageRating;
			entity.MinPrice = (decimal)from.MinPrice;
			entity.MaxPrice = (decimal)from.MaxPrice;
			entity.CreatorCountry = creatorCountry;
			entity.CreatorCompany = creatorCompany;
			entity.Brand = brand;
			entity.ProductLineup = lineup;
			entity.Tags = tags;
			entity.PetKind = petKind;
			entity.MediaURI = from.MediaURIs;
			return OperationResult.Success();
		}

		protected OperationResult SetBaseProductFields(BaseProductDto dto, BaseProductEntity from)
		{
			dto.Id = from.Id;
			dto.Name = from.Name;
			dto.Description = from.Description;
			dto.SiteArticles = from.SiteArticles;
			dto.UPC = from.UPC;
			dto.EAN = from.EAN;
			dto.AverageRating = from.AverageRating;
			dto.MinPrice = from.MinPrice;
			dto.MaxPrice = from.MaxPrice;
			//TODO Сейчас в CCName хранится русское название. Нужно унифицировать
			dto.CreatorCountryName = from.CreatorCountry.CountryNameRus;
			dto.CreatorCompanyName = from.CreatorCompany.CompanyName;
			dto.BrandName = from.Brand.BrandName;
			dto.ProductLineupName = from.ProductLineup.LineupName;
			dto.Tags = from.Tags.Select(x => x.TagName).ToList();
			dto.PetKind = from.PetKind.PetKindName;
			dto.MediaURIs = from.MediaURI;
			return OperationResult.Success();
		}
	}
}
