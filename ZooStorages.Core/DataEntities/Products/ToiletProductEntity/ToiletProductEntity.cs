using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Domain.DataEntities.Data_Primitives;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Domain.DataEntities.Products.ToiletProductEntity
{
    public enum ToiletType
    {
        Open = 0,
        Close,
        BioToilet,
        Automatic,
        Diapers
    }
    public class ToiletProductEntity : BaseProductEntity
    {
        public ToiletType ToiletType { get; set; }
        public SizeDimensions? Dimensions { get; set; }
        public float? VolumeLiters { get; set; }
        public PetAgeRange? PetAgeRange { get; set; }
    }

    public class ToiletProductEntityConfigurator : IEntityTypeConfiguration<ToiletProductEntity>
    {
        public void Configure(EntityTypeBuilder<ToiletProductEntity> builder)
        {
            builder.OwnsOne(e => e.Dimensions);
            builder.OwnsOne(e => e.PetAgeRange);
            builder.Property(e => e.ToiletType)
                .IsRequired(true);
        }
    }
}
