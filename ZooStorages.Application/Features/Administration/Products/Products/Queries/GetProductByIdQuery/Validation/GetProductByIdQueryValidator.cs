using FluentValidation;
using Microsoft.Extensions.Localization;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Application.Validation;
using ZooStorages.Domain.Localization;

namespace ZooStorages.Application.Features.Administration.Products.Products.Queries.GetProductByIdQuery.Validation
{
	public class GetProductByIdQueryValidator : BaseDtoValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator(IStringLocalizer<Validations> localizer,
            IProductsRepository products)
            : base(localizer)
        {
            RuleFor(e => e.Id)
                .Must(products.IsProductExists)
                .WithMessage(ValidationMessage("Products.ProductNotFound"));
        }

	}
}
