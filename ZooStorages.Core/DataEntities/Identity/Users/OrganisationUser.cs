using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
