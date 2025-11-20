using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Infrastructure.Parsers.Storage.ParsedDataEntities.ParsedProductsModels.Zoobazar;

namespace Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsedDataEntities.ParsedProductsModels.Zoobazar.EntityConfiguration
{
	public class ZoobazarParsedProductEntityConfigurator : IEntityTypeConfiguration<ZoobazarParsedProduct>
	{
		public void Configure(EntityTypeBuilder<ZoobazarParsedProduct> builder)
		{
			builder.HasMany(e => e.Offers);
			builder.HasMany(e => e.Tabs);
			builder.Navigation(e => e.Test).IsRequired(); 
		}
	}
}
