using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Domain.DataEntities.Data_Primitives;

namespace Zoobee.Domain.DataEntities.Products
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
		public PetFoodType? FoodType { get; set; }
		public PetAgeRange? PetAgeRange { get; set; }
		public decimal? ProductWeightGramms { get; set; }
	}

	public class FoodProductEntityConfigurator : IEntityTypeConfiguration<FoodProductEntity>
	{
		public void Configure(EntityTypeBuilder<FoodProductEntity> builder)
		{
			builder.OwnsOne(e => e.PetAgeRange);
		}
	}
}
