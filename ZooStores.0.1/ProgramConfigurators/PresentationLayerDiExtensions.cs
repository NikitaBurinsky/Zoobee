using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Zoobee.Core.Errors;
using Zoobee.Domain.Localization;

namespace Zoobee.Web.ProgramConfigurators
{
	public static class PresentationLayerDiExtensions
	{
		public static void AddPresentationLayer(this IServiceCollection services)
		{
			services.AddLocalizationResources();
			services.AddSwaggerEndpointsDocumentation();
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		}
		private static void AddLocalizationResources(this IServiceCollection services)
		{
			services.AddLocalization(r => r.ResourcesPath = "LocalizationResources");
			services.AddScoped<Errors>();
			services.AddScoped<Validations>();
			services.AddScoped<Authorization>();

			services.AddFluentValidationAutoValidation();
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		}
		private static void AddSwaggerEndpointsDocumentation(this IServiceCollection services)
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
