using FluentValidation;
using Microsoft.Extensions.Localization;
using Zoobee.Application.DTOs.Environtment.Manufactures;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Domain.Localization;

namespace Zoobee.Application.Validation.Environtment.Manufactures
{
	public class CreatorCompanyDtoValidator : BaseDtoValidator<CreatorCompanyDto>
	{
		public CreatorCompanyDtoValidator(IStringLocalizer<Validations> localizer,
			ICreatorCompaniesRepository creatorCompanies) : base(localizer)
		{
			RuleFor(e => e.CompanyName)
				.NotEmpty().WithMessage("CompanyName.MustBeNotNullOrEmpty")
				.MaximumLength(60).WithMessage("CompanyName.MaxLength60")
				.MinimumLength(3).WithMessage("CompanyName.MinLength3")
				.Must(e => !creatorCompanies.IsEntityExists(e));
		}
	}
}
