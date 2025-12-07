using FluentValidation;
using Microsoft.Extensions.Localization;
using Zoobee.Application.DTOs.Environtment.Manufactures;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Domain.Localization;

namespace Zoobee.Application.Validation.Environtment.Manufactures
{
	public class CreatorCountryDtoValidator : BaseDtoValidator<CreatorCountryDto>
	{
		public CreatorCountryDtoValidator(IStringLocalizer<Validations> localizer,
			ICreatorCountriesRepository creatorCountries) : base(localizer)
		{
			RuleFor(e => e.CountryName)
				.NotEmpty().WithMessage(ValidationMessage("CountryName.MustBeNotNullOrEmpty"))
				.MaximumLength(40).WithMessage(ValidationMessage("CountryName.MaxLength60"))
				.MinimumLength(2).WithMessage(ValidationMessage("CountryName.MinLength2"))
				.Must(e => !creatorCountries.IsEntityExists(e)).WithMessage(ValidationMessage("CountryName.SimilarNameExists"));
		}
	}
}
