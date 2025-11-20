using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Application.DtoTypes.Base;

namespace Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsedDataEntities.ParsedProductHolder
{
	[Owned]
	public abstract class ParsedProductHolder : BaseEntity
	{
		/// <summary>
		/// Когда в последний раз проходил трансфер сущности
		/// </summary>
		public Guid Id { get; set; }
		public DateTime? LastTransformed { get; set; }
		public string ParsedUrl { get; set; }
	}

	public class ParsedProductHolder<ParsedProductModel> : ParsedProductHolder
		where ParsedProductModel : class
	{
		public ParsedProductModel parsedProductModel { get; set; }
	}

	public class ParsedProductHolderEntityConfigurator<ParsedProduct> : IEntityTypeConfiguration<ParsedProductHolder<ParsedProduct>>
		where ParsedProduct : class 
	{
		public void Configure(EntityTypeBuilder<ParsedProductHolder<ParsedProduct>> builder)
		{
			builder.HasOne(e => e.parsedProductModel);
		}
	}

}
