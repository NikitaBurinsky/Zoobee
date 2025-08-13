using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Application.Models.Catalog.Product.Types;
using ZooStorages.Domain.Localization;

namespace ZooStorages.Application.Models.Catalog.Product.Types.Validation
{
    internal class ProductTypeDtoValidator : AbstractValidator<ProductTypeDto>
    {
        public IStringLocalizer<Validations> localizer;
        public string ValidationMessage(string message) => localizer[$"Validation.ProductTypes.{message}"];

        public ProductTypeDtoValidator(IStringLocalizer<Validations> localizer,
            IProductTypisationRepository productTypisationRepository)
        {
            this.localizer = localizer;
            RuleFor(e => e.TypeName)
                .MaximumLength(40)
                .WithMessage(ValidationMessage("TypeName.MaxLength40"))
                .NotNull()
                .Must(name => !productTypisationRepository.IsTypeExists(name))
                .WithMessage(ValidationMessage("TypeName.SimilarNameExists"))
                .NotEmpty()
                .WithMessage(ValidationMessage("TypeName.MustBeNotEmpty"));
            RuleFor(e => e.Information)
                .MaximumLength(200)
                .WithMessage(ValidationMessage("Information.MaxLength200"));
            RuleFor(e => e.Category)
                .Must(productTypisationRepository.IsCategoryExists)
                .WithMessage(ValidationMessage("Category.CategoryNotFound"));
        }
    }
}
