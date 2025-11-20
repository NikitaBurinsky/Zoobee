using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.ParsedDataStorage.ValueConverters.JsonElementToString;
using Zoobee.Infrastructure.Parsers.Storage.ParsedDataEntities.ParsedProductsModels.Zoobazar;

namespace Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsedDataEntities.ParsedProductsModels.Zoobazar.EntityConfiguration
{
	public class ZoobazarTagEntityConfigurator : IEntityTypeConfiguration<Tag>
	{
		public void Configure(EntityTypeBuilder<Tag> builder)
		{
			builder.Property(e => e.Id)
				.HasConversion(typeof(JsonElementToStringConverter))
				.HasColumnType("jsonb");
		}
	}
}
