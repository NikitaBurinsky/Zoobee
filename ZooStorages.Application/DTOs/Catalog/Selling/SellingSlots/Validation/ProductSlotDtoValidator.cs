using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Application.Models.Catalog.Selling.SellingSlots;
using ZooStorages.Domain.Localization;

namespace ZooStorages.Application.Models.Catalog.Selling.SellingSlots.Validation
{
    public class ProductSlotDtoValidator : AbstractValidator<ProductSlotDto>
    {
        public IStringLocalizer<Validations> localizer;
        public string ValidationMessage(string message) => localizer[$"Validation.SellingSlots.{message}"];

        public ProductSlotDtoValidator(IStringLocalizer<Validations> localizer,
            IProductSellingSlotsRepository productSellingSlots,
            IProductsRepository productsRepository)
        {
            this.localizer = localizer;

            RuleFor(e => e.DefaultSlotPrice)
                .NotNull().WithMessage(ValidationMessage("DefaultSlotPrice.MustBeNotNull"))
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage("DefaultSlotPrice.MustBePostitive"));
            RuleForEach(e => e.SelfPickupOptions)
                .Must(productSellingSlots.IsSelfPickupOptionExists)
                .WithMessage(ValidationMessage("SelfPickupOptions.OneOrMoreSelfPickupOptionsNotFound"));
            RuleForEach(e => e.DeliveryOptions)
                .Must(productSellingSlots.IsDeliveryOptionExists)
                .WithMessage(ValidationMessage("DeliveryOptions.OneOrMoreDeliveryOptionsNotFound"));
            RuleFor(e => e.ProductId)
                .Must(productsRepository.IsProductExists)
                .WithMessage(ValidationMessage("Product.ProductNotFound"));
            RuleFor(e => e.Discount)
                .Must(disc => disc >= 0 && disc <= 100)
                .WithMessage(ValidationMessage("Discount.ValueHasToBeInRangeFrom0To100"));
        }
    }
}
