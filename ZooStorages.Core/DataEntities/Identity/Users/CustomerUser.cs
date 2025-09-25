using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Zoobee.Domain.DataEntities.Identity.Users
{
    public class CustomerUser : BaseApplicationUser
    {
        public int BornYear { get; set; }
    }
    public class CustomerUserEntityConfigurator : IEntityTypeConfiguration<CustomerUser>
    {
        public virtual void Configure(EntityTypeBuilder<CustomerUser> builder)
        {
        }
    }

}
