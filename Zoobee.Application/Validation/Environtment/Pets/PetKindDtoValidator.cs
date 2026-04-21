using FluentValidation;
using Microsoft.Extensions.Localization;
using Zoobee.Application.Interfaces.Repositories.Environment.Pets;
using Zoobee.Application.Shared.DTOs.Environment.Pets;
using Zoobee.Domain.Localization;

namespace Zoobee.Application.Validation.Environment.Pets
{
	public class PetKindDtoValidator : BaseDtoValidator<PetKindDto>
	{
		public PetKindDtoValidator(IStringLocalizer<Validations> localizer,
			IPetKindsRepository petKinds) : base(localizer)
		{
			RuleFor(e => e.PetKindName)
				.NotEmpty().WithMessage(ValidationMessage("PetKindName.MustBeNotNullOrEmpty"))
				.MaximumLength(30).WithMessage(ValidationMessage("PetKindName.MaxLength30"))
				.MinimumLength(3).WithMessage(ValidationMessage("PetKindName.MinLength3"))
				.Must(x => !petKinds.IsEntityExists(x)).WithMessage(ValidationMessage("PetKindName.SimilarNameExists"));
		}
	}
}
