using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zoobee.Domain.DataEntities.Identity.Users
{
	public abstract class BaseApplicationUser : IdentityUser<Guid>
	{
	}

	public class BaseApplicationUserEntityConfigurator : IEntityTypeConfiguration<BaseApplicationUser>
	{
		public virtual void Configure(EntityTypeBuilder<BaseApplicationUser> builder)
		{
			builder.HasDiscriminator<string>("UserType")
				.HasValue<AdminUser>("Admin")
				.HasValue<CustomerUser>("Customer")
				.HasValue<OrganisationUser>("Organisation");

		}
	}

}
