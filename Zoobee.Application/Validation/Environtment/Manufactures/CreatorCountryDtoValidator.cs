using System.Linq;
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
			RuleFor(e => e.CountryNameRus)
				.NotEmpty().WithMessage(ValidationMessage("CountryName.MustBeNotNullOrEmpty"))
				.MaximumLength(40).WithMessage(ValidationMessage("CountryName.MaxLength60"))
				.MinimumLength(2).WithMessage(ValidationMessage("CountryName.MinLength2"))
				.Must(e => e.All(c => IsCyrillicLetter(c) || char.IsWhiteSpace(c) || c == '-' || c == '\''))
					.WithMessage(ValidationMessage("CountryName.OnlyRussianLetters"))
				.Must(e => !creatorCountries.IsEntityExists(e)).WithMessage(ValidationMessage("CountryName.SimilarNameExists"));


			RuleFor(e => e.CountryNameEng)
				.NotEmpty().WithMessage(ValidationMessage("CountryName.MustBeNotNullOrEmpty"))
				.MaximumLength(40).WithMessage(ValidationMessage("CountryName.MaxLength60"))
				.MinimumLength(2).WithMessage(ValidationMessage("CountryName.MinLength2"))
				.Must(e => e.All(c => IsLatinLetter(c) || char.IsWhiteSpace(c) || c == '-' || c == '\''))
					.WithMessage(ValidationMessage("CountryName.OnlyEnglishLetters"))
				.Must(e => !creatorCountries.IsEntityExists(e)).WithMessage(ValidationMessage("CountryName.SimilarNameExists"));
		}

		private static bool IsLatinLetter(char c)
		{
			// Basic ASCII Latin letters A-Z, a-z
			return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
		}

		private static bool IsCyrillicLetter(char c)
		{

			return (c >= '\u0400' && c <= '\u04FF') || (c >= '\u0500' && c <= '\u052F');
		}
	}
}
