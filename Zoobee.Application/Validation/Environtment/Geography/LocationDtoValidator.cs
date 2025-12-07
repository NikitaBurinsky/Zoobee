using Microsoft.Extensions.Localization;
using Zoobee.Application.DTOs.Environtment.Geography;
using Zoobee.Domain.Localization;

namespace Zoobee.Application.Validation.Environtment.Geography
{
	public class LocationDtoValidator : BaseDtoValidator<LocationDto>
	{
		public LocationDtoValidator(IStringLocalizer<Validations> localizer) : base(localizer)
		{



		}
	}
}
