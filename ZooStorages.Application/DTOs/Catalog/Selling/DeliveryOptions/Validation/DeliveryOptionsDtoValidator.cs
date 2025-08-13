using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Application.Models.Catalog.Selling.DeliveryOptions;
using ZooStorages.Domain.Localization;

namespace ZooStorages.Application.Models.Catalog.Selling.DeliveryOptions.Validation
{
    public class DeliveryOptionsDtoValidator : AbstractValidator<DeliveryOptionDto>
    {
        public IStringLocalizer<Validations> localizer;
        public string ValidationMessage(string message) => localizer[$"Validation.DeliveryOptions.{message}"];
        public DeliveryOptionsDtoValidator(
            IStringLocalizer<Validations> local,
            IProductSellingSlotsRepository productSellingSlots,
            IProductsRepository productsRepository)
        {
            localizer = local;
            RuleFor(e => e.DeliveryCost)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage("DeliveryCost.MustBePositiveOrNull"));
            RuleForEach(e => e.PaymentTypes)
                .MaximumLength(30).WithMessage(ValidationMessage("PaymentTypes.MaxLength30"));
            RuleForEach(e => e.ProductSlotIds)
                .Must(productSellingSlots.IsSlotExists)
                .WithMessage(ValidationMessage("ProductSlots.OneOrMoreProductSlotsNotFound")); //TODO проверить, будет ли в ответе количество невалидных из коллекции
        }
    }
}
