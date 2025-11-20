using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zoobee.Domain.DataEntities.Identity.Users
{
	public class AdminUser : BaseApplicationUser
	{

	}
	public class AdminUserEntityConfigurator : IEntityTypeConfiguration<AdminUser>
	{
		public virtual void Configure(EntityTypeBuilder<AdminUser> builder)
		{
		}
	}

}
