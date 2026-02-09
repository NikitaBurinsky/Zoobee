using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zoobee.Domain.DataEntities.Identity.Users
{
	public class CustomerUser : BaseApplicationUser
	{
		public required string NickName { get; set; }
		public int? BornYear { get; set; }
	}
	public class CustomerUserEntityConfigurator : IEntityTypeConfiguration<CustomerUser>
	{
		public virtual void Configure(EntityTypeBuilder<CustomerUser> builder)
		{
			builder.Property(e => e.NickName)
				.IsRequired(true).HasMaxLength(64);


		}
	}

}
