using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Application.Models.Catalog.Product.Categories;
using ZooStorages.Domain.Localization;

namespace ZooStorages.Application.Models.Catalog.Product.Categories.Validation
{
    public class ProductCategoryDtoValidator : AbstractValidator<ProductCategoryDto>
    {
        private string ValidationMessage(string message) => localizer[$"Validation.ProductCategories.{message}"];
        private IStringLocalizer<Validations> localizer;
        public ProductCategoryDtoValidator(
            IStringLocalizer<Validations> local,
            IProductTypisationRepository typisationRepository
            )
        {
            localizer = local;
            RuleFor(x => x.CategoryName)
                    .MaximumLength(30)
                    .WithMessage(ValidationMessage("CategoryName.MaxLength30"))
                    .Must(e => !typisationRepository.IsCategoryExists(e))
                    .WithMessage(ValidationMessage("CategoryName.SimilarNameExists"))
                    .NotEmpty()
                    .WithMessage(ValidationMessage("CategoryName.MustBeNotEmpty"));
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(ValidationMessage("Description.MustBeNotEmpty"))
                .MaximumLength(300).WithMessage("Description.MaxLength300");
        }
    }
}
