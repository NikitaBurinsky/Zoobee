using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Data_Primitives;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.UpdateProductsSpecificInfoProfiles
{
	public class FoodProductUpdateSpecInfoProfile : IUpdateProductSpecificProfile<FoodProductDto, FoodProductEntity>
	{
		public OperationResult UpdateSpecificInfo(FoodProductDto newInfo, FoodProductEntity entityToUpdate)
		{
			if (entityToUpdate.FoodType == null)
				entityToUpdate.FoodType = newInfo.FoodType;
			UpdatePetAgeRange(newInfo, entityToUpdate);
			if (entityToUpdate.ProductWeightGramms == null)
				entityToUpdate.ProductWeightGramms = newInfo.ProductWeightGrams;
			return OperationResult.Success();
		}
		private static void UpdatePetAgeRange(FoodProductDto newInfo, FoodProductEntity entityToUpdate)
		{
			if (entityToUpdate.PetAgeRange == null)
				entityToUpdate.PetAgeRange = newInfo.PetAgeRange;
			else if (entityToUpdate.PetAgeRange.PetAgeWeeksMax == null)
				entityToUpdate.PetAgeRange.PetAgeWeeksMax = newInfo.PetAgeRange.PetAgeWeeksMax;
			else if (entityToUpdate.PetAgeRange.PetAgeWeeksMin == null)
				entityToUpdate.PetAgeRange.PetAgeWeeksMin = newInfo.PetAgeRange.PetAgeWeeksMin;
		}

		public PetFoodType? FoodType { get; set; }
		public PetAgeRange? PetAgeRange { get; set; }
		public decimal? ProductWeightGramms { get; set; }
	}
}

