using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain.DataEntities.Catalog.Reviews;
using Zoobee.Domain.DataEntities.Catalog.Tags;
using Zoobee.Domain.DataEntities.Environment.Creators;
using Zoobee.Domain.DataEntities.Environment.Manufactures;

namespace Zoobee.Domain.DataEntities.Products
{
	public class BaseProductEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string NormalizedName { get; set; }
		public string Description { get; set; }
		public Dictionary<string,string> SiteArticles { get; set; } 
		public string? UPC { get; set; }
		public string? EAN { get; set; }
		public float AverageRating { get; set; }
		public decimal MinPrice { get; set; }
		public decimal MaxPrice { get; set; }
		public CreatorCountryEntity CreatorCountry { get; set; }
		public BrandEntity Brand { get; set; }
		public ProductLineupEntity ProductLineup { get; set; }
		public CreatorCompanyEntity CreatorCompany { get; set; }
		public ICollection<TagEntity> Tags;
		public PetKindEntity PetKind { get; set; }
		public ICollection<SellingSlotEntity> SellingSlots { get; set; }
		public ICollection<ReviewEntity> Reviews { get; set; }
		public List<string> MediaURI { get; set; } = new List<string>();
	}
	public class BaseProductEntityConfigurator : IEntityTypeConfiguration<BaseProductEntity>
	{
		public void Configure(EntityTypeBuilder<BaseProductEntity> builder)
		{
			builder.Property(x => x.Name)
				.HasMaxLength(30);
			builder.HasIndex(x => x.NormalizedName)
				.IsUnique(true);
			builder.Property(x => x.Description)
				.HasMaxLength(400);
			builder.Property(x => x.SiteArticles)
				.HasMaxLength(10);
			builder.Property(x => x.UPC)
				.HasMaxLength(12);
			builder.Property(x => x.EAN)
				.HasMaxLength(13);
			builder.HasOne(x => x.CreatorCompany);
			builder.HasOne(x => x.CreatorCountry);
			builder.HasOne(x => x.Brand);
			builder.HasOne(x => x.PetKind);
			builder.HasOne(x => x.ProductLineup);
			builder.HasMany(x => x.Tags);
			builder.HasMany(x => x.SellingSlots)
				.WithOne(e => e.Product);
			builder.HasMany(x => x.Reviews);
			builder.PrimitiveCollection(x => x.MediaURI);

			builder.Property(x => x.SiteArticles)
			.HasConversion(
				v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
				v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null) ?? new Dictionary<string, string>()
			)
			.Metadata.SetValueComparer(new ValueComparer<Dictionary<string, string>>(
				(c1, c2) => c1.SequenceEqual(c2),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToDictionary(entry => entry.Key, entry => entry.Value)
			));
		}
	}
}
