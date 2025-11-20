using FluentValidation;
using Microsoft.Extensions.Localization;
using Zoobee.Domain.Localization;
using ZooStores.Web.Area.Identity.Autorization.Models;

namespace ZooStores.Web.Area.Identity.Autorization.Validation
{
	public class LoginRequestValidator : AbstractValidator<LoginRequestModel>
	{
		IStringLocalizer<Validations> localizer;
		public LoginRequestValidator(IStringLocalizer<Validations> local)
		{
			localizer = local;
			RuleFor(e => e.Password)
				.NotEmpty()
					.WithMessage(localizer["Validation.Login.Password.MustBeNotNullOrEmpty"])
				.MinimumLength(8)
					.WithMessage(localizer["Validation.Login.Password.MinLength8"]);
			RuleFor(e => e.Email)
				.NotEmpty()
					.WithMessage(localizer["Validation.Login.Email.MustBeNotNullOrEmpty"])
				.EmailAddress()
					.WithMessage(localizer["Validation.Login.Email.IncorrectEmailFormat"]);
		}
	}
}
