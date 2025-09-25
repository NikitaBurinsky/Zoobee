using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DtoTypes.Base;

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
