using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zoobee.Domain.DataEntities.Identity.Users
{
	public class OrganisationUser : BaseApplicationUser
	{

	}
	public class OrganisationUserEntityConfigurator : IEntityTypeConfiguration<OrganisationUser>
	{
		public virtual void Configure(EntityTypeBuilder<OrganisationUser> builder)
		{
		}
	}

}
