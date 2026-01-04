using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.UpdateProductsSpecificInfoProfiles;

namespace Zoobee.Infrastructure.Program_Configuration.Assembly_Validation;

public class UpdateProfileValidationService
{
	private ILogger<UpdateProfileValidationService> _logger;

	public UpdateProfileValidationService()
	{ }

	public void ValidateProfilesCompleteness(WebApplication app)
	{
		using (var scope = app.Services.CreateScope())
		{
			_logger = scope.ServiceProvider.GetRequiredService<ILogger<UpdateProfileValidationService>>();
			var d = _logger.BeginScope("Infrastructure-Layer-Code-Validation");

			_logger.LogInformation("Проверка наличия всех UpdateProductSpecificProfiles...");

			var domainAssembly = typeof(BaseProductEntity).Assembly;
			var infrastructureAssembly = GetType().Assembly;

			var productEntityTypes = domainAssembly.GetTypes()
				.Where(t => !t.IsAbstract &&
						   t.IsClass &&
						   t.IsSubclassOf(typeof(BaseProductEntity)))
				.ToList();

			var profileTypes = infrastructureAssembly.GetTypes()
				.Where(t => !t.IsAbstract &&
						   t.IsClass &&
						   IsSubclassOrImplementsGeneric(typeof(IUpdateProductSpecificProfile<,>), t))
				.ToList();
			int count = productEntityTypes.Count;
			_logger.LogInformation("Найдено BaseProductEntity классов : {Count}", count);
			count = profileTypes.Count;
			_logger.LogInformation("Найдено BaseUpdateEntity классов : {Count}", count);
			var missingProfiles = new List<string>();

			foreach (var productEntityType in productEntityTypes)
			{
				var dtoType = FindCorrespondingDto(productEntityType);
				if (dtoType == null)
				{
					_logger.LogWarning("No DTO found for entity {EntityName}", productEntityType.Name);
					continue;
				}
				// НЕРАБОТАЕТ
				var profileType = profileTypes.FirstOrDefault(profileType =>
				{
					// 1. Проверяем наследование от класса (BaseType)
					var baseType = profileType.BaseType;
					if (baseType != null && baseType.IsGenericType)
					{
						var genericArgs = baseType.GetGenericArguments();
						if (genericArgs.Length == 2 &&
							genericArgs[0] == dtoType &&
							genericArgs[1] == productEntityType)
						{
							return true;
						}
					}

					// 2. Проверяем реализацию интерфейса
					var interfaces = profileType.GetInterfaces();
					foreach (var interfaceType in interfaces)
					{
						if (interfaceType.IsGenericType)
						{
							var genericArgs = interfaceType.GetGenericArguments();
							if (genericArgs.Length == 2 &&
								genericArgs[0] == dtoType &&
								genericArgs[1] == productEntityType)
							{
								return true;
							}
						}
					}

					return false;
				});

				if (profileType == null)
				{
					missingProfiles.Add($"{productEntityType.Name} -> {dtoType.Name}");
				}
			}

			if (missingProfiles.Any())
			{
				var message = $"Missing UpdateProductSpecificProfile implementations:\n" +
							 string.Join("\n", missingProfiles.Select(mp => $"  - {mp}"));

				_logger.LogCritical(message);

				throw new InvalidOperationException(message);
			}
			else
			{
				_logger.LogInformation("Проверка успешна. Профили UpdateProductSpecificProfile в наличии");
			}
		}
	}
	private Type? FindCorrespondingDto(Type entityType)
	{
		// Пытаемся найти DTO по конвенции именования
		var possibleDtoNames = new[]
		{
			entityType.Name.Replace("Entity", "Dto"),
			entityType.Name.Replace("ProductEntity", "ProductDto"),
			entityType.Name.Replace("Entity", "") + "Dto",
			entityType.Name + "Dto"
		};

		var assemblies = AppDomain.CurrentDomain.GetAssemblies()
			.Where(a => !a.IsDynamic &&
					   a.FullName != null &&
					   (a.FullName.Contains("Application") ||
						a.FullName.Contains("Infrastructure") ||
						a.FullName.Contains("Zoobee")));

		foreach (var assembly in assemblies)
		{
			foreach (var dtoName in possibleDtoNames)
			{
				try
				{
					var dtoType = assembly.GetTypes()
						.FirstOrDefault(t => t.Name == dtoName &&
										   t.Name.EndsWith("Dto"));

					if (dtoType != null)
						return dtoType;
				}
				catch (ReflectionTypeLoadException)
				{
					// Пропускаем сборки с ошибками загрузки
					continue;
				}
			}
		}

		return null;
	}

	private bool IsSubclassOrImplementsGeneric(Type generic, Type toCheck)
	{
		// Проверка наследования (для классов)
		var currentType = toCheck;
		while (currentType != null && currentType != typeof(object))
		{
			if (currentType.IsGenericType &&
				currentType.GetGenericTypeDefinition() == generic)
			{
				return true;
			}
			currentType = currentType.BaseType;
		}

		// Проверка реализации интерфейсов
		var interfaces = toCheck.GetInterfaces();
		foreach (var interfaceType in interfaces)
		{
			if (interfaceType.IsGenericType &&
				interfaceType.GetGenericTypeDefinition() == generic)
			{
				return true;
			}

			// Также проверяем интерфейсы родительских интерфейсов
			var parentInterfaces = interfaceType.GetInterfaces();
			foreach (var parentInterface in parentInterfaces)
			{
				if (parentInterface.IsGenericType &&
					parentInterface.GetGenericTypeDefinition() == generic)
				{
					return true;
				}
			}
		}

		return false;
	}
}