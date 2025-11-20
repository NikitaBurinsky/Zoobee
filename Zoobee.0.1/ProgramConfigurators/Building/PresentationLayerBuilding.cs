using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Serialization;
using Zoobee.Core.Errors;
using Zoobee.Domain.Localization;

namespace Zoobee.Web.ProgramConfigurators.Building
{
	public static class PresentationLayerBuilding
	{
		public static void AddPresentationLayer(this IServiceCollection services)
		{
			AddControllers(services);
			AddLocalizationResources(services);
			AddSwaggerEndpointsDocumentation(services);
			AddValidation(services);
			services.AddAuthorization();
			services.AddAuthentication();
		}

		public static void AddControllers(IServiceCollection services)
		{
			services.AddControllersWithViews()
				.AddJsonOptions(options =>
				{ options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });
		}

		private static void AddValidation(IServiceCollection services)
		{
			services.AddFluentValidationAutoValidation();
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		}
		private static void AddLocalizationResources(IServiceCollection services)
		{
			services.AddLocalization(r => r.ResourcesPath = "LocalizationResources");
			services.AddScoped<Errors>();
			services.AddScoped<Validations>();
			services.AddScoped<Authorization>();

		}
		private static void AddSwaggerEndpointsDocumentation(IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "File Upload API", Version = "v1" });
				c.OperationFilter<SwaggerFileUploadFilter>(); // (см. ниже)
				c.MapType<IFormFile>(() => new OpenApiSchema
				{
					Type = "string",
					Format = "binary"
				});
			});
		}

		public class SwaggerFileUploadFilter : IOperationFilter
		{
			public void Apply(OpenApiOperation operation, OperationFilterContext context)
			{
				var formFileParameters = context.MethodInfo.GetParameters()
					.Where(p => p.ParameterType == typeof(IFormFile))
					.Select(p => p.Name)
					.ToList();

				if (!formFileParameters.Any())
					return;

				operation.RequestBody = new OpenApiRequestBody
				{
					Content =
			{
				["multipart/form-data"] = new OpenApiMediaType
				{
					Schema = new OpenApiSchema
					{
						Type = "object",
						Properties = formFileParameters.ToDictionary(
							key => key,
							_ => new OpenApiSchema
							{
								Type = "string",
								Format = "binary"
							}
						)
					}
				}
			}
				};
			}
		}
	}
}
