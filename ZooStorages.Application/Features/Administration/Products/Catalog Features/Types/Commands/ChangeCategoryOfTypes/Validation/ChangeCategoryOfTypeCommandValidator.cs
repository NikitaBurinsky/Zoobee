using FluentValidation;
using Microsoft.Extensions.Localization;
using ZooStorages.Application.Features.Catalog_Features.Types.Commands;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Domain.Localization;

namespace ZooStorages.Application.Features.Administration.Products.Catalog_Features.Types.Commands.ChangeCategoryOfTypes.Validation
{
	public class ChangeCategoryOfTypeCommandValidator : AbstractValidator<ChangeCategoryOfTypeCommand>
	{
		public ChangeCategoryOfTypeCommandValidator(
			IStringLocalizer<Validations> localizer,
			IProductTypisationRepository productTypisationRepository)
		{
			RuleFor(e => e.TypeName)
				.NotNull()
					.WithMessage("Validation.ProductTypes.TypeName.MustBeNotNull")
				.Must(productTypisationRepository.IsTypeExists)
					.WithMessage("Validation.ProductTypes.TypeName.ProductType");
			RuleFor(x => x.NewCategory)
			.MaximumLength(30)
				.WithMessage("Validation.ProductTypes.NewCategory.MaxLength30")
			.Must(e => !productTypisationRepository.IsCategoryExists(e))
				.WithMessage("Validation.ProductTypes.NewCategory.SimilarNameExists")
			.NotEmpty()
				.WithMessage("Validation.ProductTypes.NewCategory.MustBeNotEmpty");
		}
	}
}
