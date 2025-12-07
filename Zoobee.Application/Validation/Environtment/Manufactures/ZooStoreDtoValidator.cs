using FluentValidation;
using Microsoft.Extensions.Localization;
using Zoobee.Application.DTOs.Environtment.Manufactures;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Domain.DataEntities.Environment.Manufactures;
using Zoobee.Domain.Localization;

namespace Zoobee.Application.Validation.Environtment.Manufactures
{
	public class ZooStoreDtoValidator : BaseDtoValidator<ZooStoreDto>
	{
		public ZooStoreDtoValidator(
			IStringLocalizer<Validations> localizer,
			ISellerCompanyRepository sellerCompanyRepository
			) : base(localizer)
		{
			RuleFor(e => e.SellerCompanyName)
				.NotNull().WithMessage(ValidationMessage("SellerCompanyName.MustBeNotNull"))
				.MaximumLength(60).WithMessage(ValidationMessage("SellerCompanyName.MaxLength60"))
				.Must(sellerCompanyRepository.IsEntityExists).WithMessage(ValidationMessage("SellerCompanyName.SellerCompanyNotFound"));
			RuleFor(e => e.Name)
				.MaximumLength(60).WithMessage(ValidationMessage("Name.MaxLength60"));

			RuleFor(dto => dto)
				.Must(dto => dto.Location != null && dto.StoreType != ZooStoreType.OnlyOnline)
					.WithMessage(ValidationMessage("Location.OnlineStoreCannotHaveLocation"))
				.Must(dto => dto.OpeningTime == null || dto.ClosingTime >= dto.OpeningTime)
					.WithMessage(ValidationMessage("ClosingTime.MustBeAfterOpening"));

			RuleFor(loc => loc.Location)
				.Must(loc => loc.Address != null && loc.City != null || loc.GeoPoint != null)
					.WithMessage("Location.MustContaingAddressOrCoordinates")
				.When(loc => loc != null);

		}
	}
}
