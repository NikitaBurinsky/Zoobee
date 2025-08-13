using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Application.Models.Catalog.Selling.SelfPickupOptions;
using ZooStorages.Domain.Localization;

namespace ZooStorages.Application.Models.Catalog.Selling.SelfPickupOptions.Validation
{
    public class SelfPickupOptionsDtoValidator : AbstractValidator<SelfPickupOptionDto>
    {
        public IStringLocalizer<Validations> localizer;
        public string ValidationMessage(string message) => localizer[$"Validation.SelfPickupOptions.{message}"];
        public SelfPickupOptionsDtoValidator(
            IStringLocalizer<Validations> local,
            IProductSellingSlotsRepository productSellingSlotsRepository)
        {
            localizer = local;
            RuleFor(e => e.DeliveryToPointCost)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage("DeliveryToPointCost.MustBePositiveOrNull"));
            RuleForEach(e => e.PaymentTypes)
                .MaximumLength(30).WithMessage(ValidationMessage("PaymentTypes.MaxLength30"));
            RuleForEach(e => e.ProductSlotIds)
                .Must(productSellingSlotsRepository.IsSlotExists)
                .WithMessage(ValidationMessage("ProductSlots.OneOrMoreProductSlotsNotFound")); //TODO проверить, будет ли в ответе количество невалидных из коллекции
        }

    }
}
