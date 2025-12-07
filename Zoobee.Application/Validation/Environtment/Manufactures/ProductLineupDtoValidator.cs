using FluentValidation;
using Microsoft.Extensions.Localization;
using Zoobee.Application.DTOs.Environtment.Manufactures;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Domain.Localization;

namespace Zoobee.Application.Validation.Environtment.Manufactures
{
	public class ProductLineupDtoValidator : BaseDtoValidator<ProductLineupDto>
	{
		public ProductLineupDtoValidator(IStringLocalizer<Validations> localizer,
			IProductLineupRepository productLineups,
			IBrandsRepository brandsRepository) : base(localizer)
		{
			RuleFor(e => e.BrandName)
				.NotEmpty().WithMessage(ValidationMessage("BrandName.MustBeNotNullOrEmpty"))
				.MaximumLength(60).WithMessage(ValidationMessage("BrandName.MaxLength60"))
				.Must(brandsRepository.IsEntityExists).WithMessage(ValidationMessage("BrandName.BrandNotFound"));
			RuleFor(e => e.LineupName)
				.NotEmpty().WithMessage(ValidationMessage("LineupName.MustBeNotNullOrEmpty"))
				.MaximumLength(60).WithMessage(ValidationMessage("LineupName.MaxLength60"))
				.MinimumLength(2).WithMessage(ValidationMessage("LineupName.MinLength2"));
			RuleFor(e => e.LineupDescription)
				.NotNull().WithMessage(ValidationMessage("LineupName.MustBeNotNull"))
				.MaximumLength(60).WithMessage(ValidationMessage("LineupName.MaxLength60"));
			RuleFor(e => e)
				.Must(e => !productLineups.IsEntityExists(e.BrandName, e.LineupName))
				.WithMessage(ValidationMessage("LineupName.BrandContainsSimilarName"));
		}
	}
}
