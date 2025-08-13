using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Domain.Localization;

namespace ZooStorages.Application.Validation
{
	public abstract class BaseDtoValidator<T> : AbstractValidator<T>
		where T : class
	{
		public string ValidationMessage(string message)
			=> localizer[$"Validation.{nameof(T)}.{message}"];

		public IStringLocalizer<Validations> localizer;
		public BaseDtoValidator(IStringLocalizer<Validations> localizer)
		{
			this.localizer = localizer;
		}
	}
}
