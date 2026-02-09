using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zoobee.Domain.DataEntities.Identity.Users
{
	public class AdminUser : BaseApplicationUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; } 
		public string PatronymicName { get; set; }
	}

	public class AdminUserEntityConfigurator : IEntityTypeConfiguration<AdminUser>
	{
		public virtual void Configure(EntityTypeBuilder<AdminUser> builder)
		{
			builder
				.HasIndex(x => x.FirstName + x.LastName + x.PatronymicName)
				.IsUnique(true);

			builder.Property(e => e.FirstName)
				.HasMaxLength(40);
			builder.Property(e => e.PatronymicName)
				.HasMaxLength(40);
			builder.Property(e => e.LastName)
				.HasMaxLength(40);
			
		}
	}

}
