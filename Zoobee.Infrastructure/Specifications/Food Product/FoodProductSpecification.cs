using LinqKit;
using Zoobee.Application.DTOs.Filters;
using Zoobee.Domain.DataEntities.Products.FoodProductEntity;
using Zoobee.Infrastructure.Specifications.Base;

namespace Zoobee.Application.Filtration.Food_Products
{
	public class FoodProductFilterSpecification : BaseProductSpecification<FoodProductEntity>
	{
		public FoodProductFilterSpecification(FoodProductFilterDto filter,
			bool paginate = false, int pageSize = 10, int pageNum = 1)
			: base(filter, paginate, pageSize, pageNum)
		{
			if (filter.FoodTypes != null && filter.FoodTypes.Any())
			{
				var pred = PredicateBuilder.New<FoodProductEntity>(false);
				foreach (var foodType in filter.FoodTypes)
				{
					pred.Or(ent => ent.FoodType == foodType);
				}
				Criteria.And(pred);
			}

			if (filter.PetAgeRange != null)
			{
				Criteria.And(e => e.PetAgeRange.IsRangeInsideOf(filter.PetAgeRange));
			}

			if (filter.ProductWeightGrammsRange != null)
			{
				Criteria.And(e =>
				e.ProductWeightGramms <= filter.ProductWeightGrammsRange.Max
				&& e.ProductWeightGramms >= filter.ProductWeightGrammsRange.Min);
			}
		}
	}
}
