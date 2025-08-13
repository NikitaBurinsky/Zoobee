using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Application.Interfaces.Services.ProductsAdminitrator;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStorages.Domain.DataEntities.Products.Components.Attributes;
using ZooStorages.Domain.Enums;
using ZooStorages.Application.IQueriableExtensions;
using Microsoft.EntityFrameworkCore;
using ZooStorages.Application.Models.Catalog.Product.Attributes;
using ZooStorages.Application.Models.Catalog.Product.Product;

namespace ZooStorages.Infrastructure.Services
{
    public sealed class ProductsAdministratorService : IProductsAdministratorService
	{
		public IUnitOfWork unitOfWork;
		public IStringLocalizer<Errors> localizer;

		public ProductsAdministratorService(IUnitOfWork unitOfWork, IStringLocalizer<Errors> localizer)
		{
			this.unitOfWork = unitOfWork;
			this.localizer = localizer;
		}

		public async Task<OperationResult<Guid>> CreateProductAsync(ProductDto productDto)
		{
			var manufactureAtts = CreateManufactureAsync(productDto.ManufactureAttributes);
			var physicalAtts = CreatePhysicalAttributesAsync(productDto.PhysicalAttributes);
			var petAtts = await CreatePetAttributesAsync(productDto.PetInfoAttributes);
			if (manufactureAtts != null &&
				physicalAtts != null &&
				petAtts != null)
			{
				var productType = await unitOfWork.ProductTypes.GetProductTypeAsync(productDto.ProductType);
				if (productType == null)
					return OperationResult<Guid>.Error(localizer["Error.Products.ProductTypeNotFound"], HttpStatusCode.InternalServerError);

				var product = new ProductEntity
				{
					Name = productDto.Name.Trim(),
					Information = productDto.Information.Trim(),
					MediaURI = productDto.MediaURI, 
					ManufactureAttributes = manufactureAtts,
					PetInfoAttributes = petAtts,
					PhysicalAttributes = physicalAtts,
					ProductType = productType,
					ExtendedAttributes = await unitOfWork.DynamicAttributes.CreateAttributesValuesAsync(productDto.ExternalAttributes)
				};
				return await unitOfWork.Products.CreateProductAsync(product);
			}
			return OperationResult<Guid>.Error(localizer["Error.Products.InvalidData"], HttpStatusCode.BadRequest);
		}
		public OperationResult<List<ProductDto>> ListProductsUnordered(int pagenum, int pagesize)
		{
			if (pagenum < 1 || pagesize < 1)
				return OperationResult<List<ProductDto>>.Error(localizer["Error.Products.IncorrectPaginateArguments"], HttpStatusCode.BadRequest);
			var list = unitOfWork.Products.Products
				.Include(e => e.ExtendedAttributes)
				.ThenInclude(e => e.AttributeType)
				.Include(e => e.ProductType)
				.Include(e => e.PetInfoAttributes.PetKind)
				.Paginate(pagenum, pagesize)
				.Select(e => ProductDto.FromEntity(e))
				.ToList();
			return OperationResult<List<ProductDto>>.Success(list);
		}

		private ManufactureAttributes CreateManufactureAsync(ManufactureAttributesDto manufactureAttributes)
		{
			if (manufactureAttributes == null)
				return null;
			return new ManufactureAttributes
			{
				Brand = manufactureAttributes.Brand.Trim(),
				CreatorCountry = manufactureAttributes.CreatorCountry.Trim(),
				EAN_Code = manufactureAttributes.EAN_Code,
				UPC_Code = manufactureAttributes.UPC_Code,
			};
		}
		private PhysicalAttributes CreatePhysicalAttributesAsync(PhysicalAttributesDto physicalAttributes)
		{
			if (physicalAttributes == null)
				return null;
			physicalAttributes.Materials.ForEach(e => e.Trim());
			return new PhysicalAttributes
			{
				Dimensions = physicalAttributes.Dimensions,
				Color = physicalAttributes.Color.Trim(),
				Materials = physicalAttributes.Materials,
				ContentMeasurementsUnits = (ContentMeasurementUnits)physicalAttributes.ContentMeasurementsUnits,
				WeightOfProducts = physicalAttributes.WeightOfProducts,
			};
		}
		private async Task<PetProductAttributes> CreatePetAttributesAsync(PetAttributesDto petProductAttributes)
		{
			if (petProductAttributes == null)
				return null;
			var petKind = await unitOfWork.PetKinds.GetPetKindAsync(petProductAttributes.PetKind);
			if (petKind == null) return null;
			return new PetProductAttributes
			{
				PetAgeWeeksMax = petProductAttributes.PetAgeWeeksMax,
				PetAgeWeeksMin = petProductAttributes.PetAgeWeeksMin,
				PetGender = (PetGender)petProductAttributes.PetGender,
				PetKind = petKind,
				PetSize = (PetSize)petProductAttributes.PetSize,
			};
		}

	}
}
