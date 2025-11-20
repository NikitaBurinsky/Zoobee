using LinqKit;
using Zoobee.Application.DTOs.Filters.Products.Toilet_Product;
using Zoobee.Domain.DataEntities.Products.ToiletProductEntity;
using Zoobee.Infrastructure.Specifications.Base;

namespace Zoobee.Infrastructure.Specifications.Toilet_Product
{
	public class ToiletProductFilterSpecification : BaseProductSpecification<ToiletProductEntity>
	{
		public ToiletProductFilterSpecification(ToiletProductFilterDto filter,
			bool paginate = false, int pageSize = 10, int pageNum = 1)
			: base(filter, paginate, pageSize, pageNum)
		{
			if (filter.ToiletTypes != null)
			{
				var pred = PredicateBuilder.New<ToiletProductEntity>(false);
				foreach (var type in filter.ToiletTypes)
				{
					pred.Or(ent =>
					ent.ToiletType == type);
				}
				Criteria.And(pred);
			}
			if (filter.DimensionsRange != null)
			{
				Criteria.And(entity =>
				entity.Dimensions.AllLessOrEqual(filter.DimensionsRange.Max)
				&& filter.DimensionsRange.Min.AllLessOrEqual(entity.Dimensions));
			}
			if (filter.VolumeLitersRange != null)
			{
				Criteria.And(ent =>
				ent.VolumeLiters <= filter.VolumeLitersRange.Max
				&& ent.VolumeLiters >= filter.VolumeLitersRange.Min);
			}
			if (filter.PetAgeRange != null)
			{
				Criteria.And(ent =>
				ent.PetAgeRange.IsRangeInsideOf(filter.PetAgeRange));
			}
		}




	}
}
