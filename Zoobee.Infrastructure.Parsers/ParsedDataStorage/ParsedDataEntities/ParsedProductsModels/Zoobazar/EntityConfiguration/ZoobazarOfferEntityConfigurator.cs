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
	public class ZoobazarOfferEntityConfigurator : IEntityTypeConfiguration<Offer>
	{
		public void Configure(EntityTypeBuilder<Offer> builder)
		{
			builder.HasKey(e => e.Id);
			builder.HasMany(e => e.Properties);
			builder.HasMany(e => e.Tags);
			builder.Property(e => e.Sale)
				.HasConversion<JsonElementToStringConverter>()
				.HasColumnType("jsonb");
		}
	}
}
