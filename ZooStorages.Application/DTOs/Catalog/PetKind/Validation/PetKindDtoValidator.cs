using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Application.Models.Catalog.PetKind;
using ZooStorages.Domain.Localization;

namespace ZooStorages.Application.Models.Catalog.PetKind.Validation
{
    public class PetKindDtoValidator : AbstractValidator<PetKindDto>
    {
        public IStringLocalizer<Validations> localizer;
        public string ValidationMessage(string message) => localizer[$"Validation.PetKinds.{message}"];
        public PetKindDtoValidator(IStringLocalizer<Validations> localizer,
            IPetKindsRepository petKindsRepository)
        {
            this.localizer = localizer;

            RuleFor(e => e.PetKindName)
                .MaximumLength(50).WithMessage(ValidationMessage("PetKindName.MaxLength50"))
                .Must(name => !petKindsRepository.IsPetKindExists(name))
                .WithMessage(ValidationMessage("PetKindName.SimilarNameExists"))
                .Must(name => name.All(ch => !char.IsDigit(ch)))
                .WithMessage(ValidationMessage("PetKindName.MustNotContainDigits"));
        }



    }
}
