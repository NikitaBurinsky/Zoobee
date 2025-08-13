using FluentValidation;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.Localization;

namespace ZooStores.Web.ProgramConfigurators
{
	public static class PresentationLayerDiExtensions
	{
		public static void AddLocalizationResources(this IServiceCollection services)
		{
			services.AddLocalization(r => r.ResourcesPath = "LocalizationResources");
			services.AddScoped<Errors>();
			services.AddScoped<Validations>();
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		}

		public static void AddSwaggerEndpointsDocumentation(this IServiceCollection services)
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
