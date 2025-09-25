using FluentValidation;
using Microsoft.Extensions.Localization;
using Zoobee.Domain.Localization;
using ZooStores.Web.Area.Identity.Autorization.Models;

namespace ZooStores.Web.Area.Identity.Autorization.Validation
{
    public class RegistrationRequestValidator : AbstractValidator<RegistrationRequestModel>
    {
        IStringLocalizer<Validations> localizer;

        public RegistrationRequestValidator(IStringLocalizer<Validations> localiz)
        {
            localizer = localiz;
            RuleFor(x => x.BornYear)
                .GreaterThan(1900)
                    .WithMessage(localizer["Validation.Registration.BornYear.InvalidData"])
                .LessThan(DateTime.Now.Year)
                    .WithMessage(localizer["Validation.Registration.BornYear.InvalidData"]);
            RuleFor(x => x.Email)
                .EmailAddress()
                    .WithMessage(localizer["Validation.Registration.Email.InvalidFormat"]);
            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage(localizer["Validation.Registration.Password.MustBeNotNull"])
                .MinimumLength(8)
                    .WithMessage(localizer["Validation.Registration.Password.MinLength8"]);
            RuleFor(x => x.PasswordConfirmation)
                .NotEmpty()
                    .WithMessage(localizer["Validation.Registration.Password.MustBeNotNull"])
                .MinimumLength(8)
                    .WithMessage(localizer["Validation.Registration.Password.MinLength8"])
                .Must((req, pasc) => req.Password == pasc)
                    .WithMessage(localizer["Validation.Registration.Password.PasswordsAreNotConfirmed"]);
        }
    }
}
