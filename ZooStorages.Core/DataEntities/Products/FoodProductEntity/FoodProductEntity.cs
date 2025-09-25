using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Domain.DataEntities.Data_Primitives;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Domain.DataEntities.Products.FoodProductEntity
{
    public enum PetFoodType
    {
        Dry,
        Wet,
        Veterinary,
        Holistic,
        Treat
    }
    public enum PetFoodTaste
    {
        Chicken,
        Fish,
        Beef,
        Pork,
        Rabbit,
    }
    public enum PetFoodClass
    {
        Econom,
        Premium,
        Holistic
    }
    public class FoodProductEntity : BaseProductEntity
    {
        public PetFoodType FoodType { get; set; }
        public PetAgeRange PetAgeRange { get; set; }
        public decimal ProductWeightGramms { get; set; }
    }

    public class FoodProductEntityConfigurator : IEntityTypeConfiguration<FoodProductEntity>
    {
        public void Configure(EntityTypeBuilder<FoodProductEntity> builder)
        {
            builder.OwnsOne(e => e.PetAgeRange);
        }
    }
}
