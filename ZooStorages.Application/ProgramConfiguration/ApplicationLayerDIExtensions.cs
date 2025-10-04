using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Mapping_Profiles.Products;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Application.Interfaces.Repositories;
using Zoobee.Application.Interfaces.Services.Products.ProductsMapperService.Mapping_Profiles;
using Zoobee.Domain.DataEntities.Products.FoodProductEntity;
using Zoobee.Domain.DataEntities.Products.ToiletProductEntity;

namespace Zoobee.Application.ServiceCollectionExtensions
{
	public static class ApplicationLayerDependencyInjection
	{
		public static void AddApplicationLayer(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
			services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
			services.AddFluentValidationAutoValidation(
				cfg => cfg.DisableDataAnnotationsValidation = true);
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		}

		private static void AddApplicationMappingProfiles(IServiceCollection services)
		{
			services.AddScoped<IBaseMappingProfile<FoodProductDto, FoodProductEntity>, FoodProductDtoMappingProfile>();
			services.AddScoped<IBaseMappingProfile<ToiletProductDto, ToiletProductEntity>, ToiletProductDtoMappingProfile>();
		}
	}
}
