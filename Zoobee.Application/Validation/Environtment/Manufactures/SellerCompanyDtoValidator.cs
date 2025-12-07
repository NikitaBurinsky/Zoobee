using FluentValidation;
using Microsoft.Extensions.Localization;
using Zoobee.Application.DTOs.Environtment.Manufactures;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Domain.Localization;

namespace Zoobee.Application.Validation.Environtment.Manufactures
{
	public class SellerCompanyDtoValidator : BaseDtoValidator<SellerCompanyDto>
	{
		public SellerCompanyDtoValidator(IStringLocalizer<Validations> localizer,
			ISellerCompanyRepository sellerCompanyRepository) : base(localizer)
		{
			RuleFor(e => e.CompanyName)
				.NotEmpty().WithMessage(ValidationMessage("CompanyName.MustBeNotNullOrEmpty"))
				.MaximumLength(60).WithMessage(ValidationMessage("CompanyName.MaxLength60"))
				.MinimumLength(2).WithMessage(ValidationMessage("CompanyName.MinLength2"))
				.Must(name => !sellerCompanyRepository.IsEntityExists(name)).WithMessage(ValidationMessage("CompanyName.SimilarNameExists"));




		}
	}
}
