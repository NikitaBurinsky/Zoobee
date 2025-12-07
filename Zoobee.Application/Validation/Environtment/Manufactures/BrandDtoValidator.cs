using FluentValidation;
using Microsoft.Extensions.Localization;
using Zoobee.Application.DTOs.Environtment.Manufactures;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Domain.Localization;

namespace Zoobee.Application.Validation.Environtment.Manufactures
{
	public class BrandDtoValidator : BaseDtoValidator<BrandDto>
	{
		public BrandDtoValidator(IStringLocalizer<Validations> localizer,
			IBrandsRepository brandsRepository) : base(localizer)
		{
			RuleFor(e => e.BrandName)
					.NotEmpty().WithMessage(ValidationMessage("BrandName.MustBeNotNullOrEmpty"))
					.MaximumLength(60).WithMessage(ValidationMessage("BrandName.MaxLength60"))
					.MinimumLength(2).WithMessage(ValidationMessage("BrandName.MinLength2"))
					.Must(e => !brandsRepository.IsEntityExists(e)).WithMessage("BrandName.SimilarNameExists");
			RuleFor(e => e.Description)
					.NotNull().WithMessage(ValidationMessage("BrandName.MustBeNotNull"))
					.MaximumLength(750).WithMessage(ValidationMessage("BrandName.MaxLength750"));
		}
	}
}
