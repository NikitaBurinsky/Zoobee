using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.Interfaces.Services.ProductTypeRegistry;

namespace Zoobee.Infrastructure.Program_Configuration.Assembly_Validation.Validators
{
	public class ProductTypesRegistryValidation
	{
		/// <summary>
		/// Проверяет, что все типы продуктов в проекте зарегистрированы в ProductTypeRegistry
		/// </summary>
		public IApplicationBuilder ValidateProductTypeRegistrations(IApplicationBuilder app)
		{
			using var scope = app.ApplicationServices.CreateScope();
			var registry = scope.ServiceProvider.GetRequiredService<IProductTypeRegistryService>();
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<ProductTypeRegistry>>();

			// Находим все DTO типы продуктов в сборке
			var productDtoTypes = Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(BaseProductDto)))
				.ToList();

			var missingRegistrations = new List<Type>();

			foreach (var productType in productDtoTypes)
			{
				if (!registry.IsRegistered(productType))
				{
					missingRegistrations.Add(productType);
				}
			}

			if (missingRegistrations.Any())
			{
				var message = $"Missing product type registrations: {string.Join(", ", missingRegistrations.Select(t => t.Name))}";
				logger.LogCritical(message);
				throw new InvalidOperationException(message);
			}

			logger.LogInformation("All {Count} product types are properly registered", productDtoTypes.Count);

			return app;
		}
	}
}
