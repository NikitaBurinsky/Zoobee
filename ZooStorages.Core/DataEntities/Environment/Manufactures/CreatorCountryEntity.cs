using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DtoTypes.Base;

namespace Zoobee.Domain.DataEntities.Environment.Creators
{
    public class CreatorCountryEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string CountryName { get; set; }
        public string NormalizedCountryName { get; set; }
    }

	public class CreatorCountryEntityConfigurator : IEntityTypeConfiguration<CreatorCountryEntity>
	{
		public void Configure(EntityTypeBuilder<CreatorCountryEntity> builder)
		{
			builder.Property(e => e.CountryName)
				.IsRequired(true)
				.HasMaxLength(40);


		}
	}

}
