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
using ZooStorages.Application.Interfaces.Repositories;

namespace ZooStorages.Application.ServiceCollectionExtensions
{
	public static class ApplicationLayerDependencyInjection
	{
		public static void AddApplicationLayer(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
			services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
			services.AddFluentValidationAutoValidation();
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		}



	}
}
