using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Application.Interfaces.Services;
using ZooStorages.Application.Models.Catalog.Product.Product;
using ZooStorages.Application.Validation;
using ZooStorages.Domain.Enums;
using ZooStorages.Domain.Localization;

namespace ZooStorages.Application.Models.Catalog.Product.Product.Validation
{
    public class ProductDtoValidator : BaseDtoValidator<ProductDto>
    {
		public ProductDtoValidator(
            IStringLocalizer<Validations> local,
            IProductTypisationRepository productTypesRepos,
            IPetKindsRepository petKindsRepos,
            IMediaStorageService mediaStorageService) : base(local)
        {
            localizer = local;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationMessage("Name.MustBeNotEmpty"))
                .Must(e => char.IsLetter(e[0])).WithMessage(ValidationMessage("Name.MustStartWithLetter"));

            RuleFor(x => x.Information)
                .MaximumLength(300).WithMessage(ValidationMessage("Information.MaxLength300"))
                .Must(e => !string.IsNullOrWhiteSpace(e)).WithMessage(ValidationMessage("Information.MustBeNotEmpty"));

            RuleFor(x => x.ProductType)
                .Must(productTypesRepos.IsTypeExists)
                .WithMessage(ValidationMessage("ProductType.ProductTypeNotFound"));

            RuleForEach(x => x.MediaURI)
                .Must(mediaStorageService.IsMediaExists)
                .WithMessage(ValidationMessage("MediaURIs.MediaAreNotExist"));

            //Manufacture attributes
            RuleFor(x => x.ManufactureAttributes.EAN_Code)
                .Length(13).WithMessage(ValidationMessage("EAN.MustBeWithLength13"))
                .Must(x => x.All(char.IsDigit)).WithMessage(ValidationMessage("EAN.MustContainDigitsOnly")); ;
            RuleFor(x => x.ManufactureAttributes.UPC_Code)
                .Length(12).WithMessage(ValidationMessage("UPC.MustBeWithLength12"))
                .Must(x => x.All(char.IsDigit)).WithMessage(ValidationMessage("UPC.MustContainDigitsOnly"));
            RuleFor(x => x.ManufactureAttributes.CreatorCountry)
                .MaximumLength(20).WithMessage(ValidationMessage("CreatorCountry.MaxLength20"))
                .Must(x => x.All(char.IsLetter)).WithMessage(ValidationMessage("CreatorCountry.MustContainLettersOnly"));
            RuleFor(x => x.ManufactureAttributes.Brand)
                .MaximumLength(25).WithMessage(ValidationMessage("Brand.MaxLength25"))
                .Must(x => x.All(e => char.IsLetter(e) || char.IsWhiteSpace(e))).WithMessage(ValidationMessage("Brand.MustContainLettersOnly"));

            //Physical attributes
            RuleFor(x => x.PhysicalAttributes.ContentMeasurementsUnits)
                .LessThan((uint)ContentMeasurementUnits.MaximalValue)
                .WithMessage(ValidationMessage("ContentMeasurementUnits.IncorrectValue"));
            RuleFor(x => x.PhysicalAttributes.Dimensions)
                .Must(x => (x.Width == null || x.Width >= 0)
                        && (x.Heigth == null || x.Heigth >= 0)
                        && (x.Length == null || x.Length >= 0))
                        .WithMessage(ValidationMessage("Dimensions.MustBePositiveOrNull"));
            RuleFor(x => x.PhysicalAttributes.WeightOfProducts)
                .GreaterThan(0).WithMessage(ValidationMessage("WeigthOfProduct.MustBePositiveOrNull"));
            RuleForEach(x => x.PhysicalAttributes.Materials)
                .NotEmpty().WithMessage(ValidationMessage("Materials.MustBeNotEmpty"));
            RuleFor(x => x.PhysicalAttributes.Color)
                .NotEmpty().WithMessage(ValidationMessage("Color.MustBeNotEmpty"))
                .Must(s => s == null || s.All(char.IsLetter)).WithMessage(ValidationMessage("Color.MustContainLettersOnly"));

            //Pet Attributes
            RuleFor(x => x.PetInfoAttributes.PetGender)
                .LessThan((uint)PetGender.MaximalValue).WithMessage(ValidationMessage("PetGender.IncorrectValue"));
            RuleFor(x => x.PetInfoAttributes.PetSize)
                .LessThan((uint)PetSize.MaximalValue).WithMessage(ValidationMessage("PetSize.IncorrectValue"));
            RuleFor(x => x.PetInfoAttributes.PetAgeWeeksMin)
                .LessThan(x => x.PetInfoAttributes.PetAgeWeeksMax)
                .WithMessage(ValidationMessage("PetAge.MinAgeMustBeLessThanMax"));
            RuleFor(x => x.PetInfoAttributes.PetKind)
                .Must(petKindsRepos.IsPetKindExists)
                .WithMessage(ValidationMessage("PetKind.PetKindNotFound"));
        }

	}
}
