using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Zoobee.Application.DTOs.Catalog;
using Zoobee.Application.Interfaces.Repositories.Catalog;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Domain.DataEntities.Identity.Users;
using Zoobee.Domain.Localization;

namespace Zoobee.Application.Validation.Catalog
{
	public class ReviewDtoValidator : BaseDtoValidator<ReviewDto>
	{
		public ReviewDtoValidator(
			IReviewsRepository reviewsRepository,
			IBaseProductsRepository productRepository,
			UserManager<BaseApplicationUser> userManager,
			IStringLocalizer<Validations> localizer) : base(localizer)
		{
			RuleFor(id => id.ReviewerUserId)
				.Must(id => userManager.Users
					.OfType<CustomerUser>().FirstOrDefault(u => u.Id == id) != null)
						.WithMessage(ValidationMessage("ReviewerUserId.UserNotFound"));
			RuleFor(x => x.Rating)
				.GreaterThanOrEqualTo(0)
					.WithMessage("Rating.MustBeGreaterOrEqualThan5")
				.LessThanOrEqualTo(5)
					.WithMessage(ValidationMessage("Rating.MustBeLessOrEqualThan0"));

			RuleFor(x => x.Text)
				.NotEmpty()
					.WithMessage(ValidationMessage("Text.MustBeNotNullOrEmpty"))
				.MaximumLength(750)
					.WithMessage(ValidationMessage("Text.MaxLength750"));

			RuleFor(e => e.ReviewedProductId)
				.Must(productRepository.IsProductExists)
					.WithMessage(ValidationMessage("ReviewedProductId.ProductNotFound"));
		}
	}
}
