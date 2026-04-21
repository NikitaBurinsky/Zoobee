using Microsoft.Extensions.Localization;
using Zoobee.Application.Shared.DTOs.Environment.Geography;
using Zoobee.Domain.Localization;

namespace Zoobee.Application.Validation.Environment.Geography
{
	public class LocationDtoValidator : BaseDtoValidator<LocationDto>
	{
		public LocationDtoValidator(IStringLocalizer<Validations> localizer) : base(localizer)
		{



		}
	}
}
