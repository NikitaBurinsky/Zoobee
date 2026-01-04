using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Program_Configuration.Assembly_Validation.Validators;

namespace Zoobee.Infrastructure.Program_Configuration.Assembly_Validation
{
	/// <summary>
	/// Валидация на старте кодовой базы
	/// Проверки на создание полного количества профилей, методов и т.д., необходимых для запуска приложения
	/// </summary>
	public static class InfrastructureLayerValidation
	{
		public static void InfrastructureAssemplyCheck(this WebApplication app)
		{
			new UpdateProfileValidationService().ValidateProfilesCompleteness(app);
			new ProductTypesRegistryValidation().ValidateProductTypeRegistrations(app);
		}


	}
}
