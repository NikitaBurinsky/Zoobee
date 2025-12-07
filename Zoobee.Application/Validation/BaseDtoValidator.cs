using FluentValidation;
using Microsoft.Extensions.Localization;
using Zoobee.Domain.Localization;

namespace Zoobee.Application.Validation
{
	public abstract class BaseDtoValidator<T> : AbstractValidator<T>
		where T : class
	{
		public virtual string ValidationMessageObjectName { get; set; } = nameof(T);
		public string ValidationMessage(string message)
			=> localizer[$"Validation.{ValidationMessageObjectName}.{message}"];

		public IStringLocalizer<Validations> localizer;
		public BaseDtoValidator(IStringLocalizer<Validations> localizer)
		{
			this.localizer = localizer;
		}
	}
}
