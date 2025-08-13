using FluentValidation;
using Microsoft.Extensions.Localization;
using ZooStorages.Application.Features.Catalog_Features.Categories.Commands;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Application.Validation;
using ZooStorages.Domain.Localization;

namespace ZooStorages.Application.Features.Administration.Products.Catalog_Features.Categories.Commands.UpdateProductCategory.Validation
{
	
	public class UpdateProductCategoryCommandValidator : BaseDtoValidator<UpdateProductCategoryCommand>
	{
		public UpdateProductCategoryCommandValidator(IStringLocalizer<Validations> localizer,
			IProductTypisationRepository productTypisationRepository) : base(localizer)
		{
			RuleFor(e => e.oldName)
				.NotNull().WithMessage(ValidationMessage("CategoryName.MustBeNotNull"))
				.Must(productTypisationRepository.IsCategoryExists)
				.WithMessage(ValidationMessage("CategoryName.ProductCategoryNotFound"));
			RuleFor(e => e.newName)
				.NotNull().WithMessage(ValidationMessage("NewCategoryName.MustBeNotNull"))
				.NotEmpty().WithMessage(ValidationMessage("NewCategoryName.MustBeNotEmpty"))
				.Must(name => !productTypisationRepository.IsCategoryExists(name))
				.WithMessage(ValidationMessage("NewCategoryName.SimilarNameExists"));
			RuleFor(x => x.newDescription)
				.MaximumLength(300).WithMessage("NewDescription.MaxLength300");
		}

	}
}
