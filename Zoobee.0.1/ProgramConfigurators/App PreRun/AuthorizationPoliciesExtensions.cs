using Zoobee.Application.Shared.Constants;

csharp ..\Zoobee.Web\Views\Authorization\AuthorizationPolicyExtensions.cs
using Microsoft.Extensions.DependencyInjection;
using Zoobee.Application.Shared.Constants;

namespace Zoobee.Web.Configuration
{
	public static class AuthorizationPolicyExtensions
	{
		public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
		{
			services.AddAuthorization(options =>
			{
				options.AddPolicy(PolicyNames.DatabaseAdmin, policy =>
					policy.RequireRole(UserRoles.DatabaseAdmin, UserRoles.SuperAdmin));
				options.AddPolicy(PolicyNames.SuperAdminOnly, policy =>
					policy.RequireRole(UserRoles.SuperAdmin));	
			});

			return services;
		}
	}
}