using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using Zoobee.Domain.Enums;
using Zoobee.Application.DtoTypes.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Domain.DataEntities.Catalog.Reviews;
using Zoobee.Domain.DataEntities.Environment.Creators;
using Zoobee.Domain.DataEntities.Environment.Manufactures;
using Zoobee.Domain.DataEntities.Catalog.Tags;

namespace Zoobee.Domain.DataEntities.Products
{
    public class BaseProductEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public string Article { get; set; }
        public string UPC { get; set; }
        public string EAN { get; set; }
        public float Rating { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public CreatorCountryEntity CreatorCountry { get; set; }
        public BrandEntity Brand { get; set; }
        public ProductLineupEntity ProductLineUp { get; set; }
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
            builder.Property(x => x.Article)
                .HasMaxLength(10);
            builder.Property(x => x.UPC)
                .HasMaxLength(12);
            builder.Property(x => x.EAN)
                .HasMaxLength(13);
            builder.HasOne(x => x.CreatorCompany);
            builder.HasOne(x => x.CreatorCountry);
            builder.HasOne(x => x.Brand);
            builder.HasOne(x => x.PetKind);
            builder.HasOne(x => x.ProductLineUp);
            builder.HasMany(x => x.Tags);
            builder.HasMany(x => x.SellingSlots)
                .WithOne(e => e.Product);
            builder.HasMany(x => x.Reviews);
            builder.PrimitiveCollection(x => x.MediaURI);
        }
    }
}
