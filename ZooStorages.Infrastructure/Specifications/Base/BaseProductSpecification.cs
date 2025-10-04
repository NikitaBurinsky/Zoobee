using LinqKit;
using System.Linq.Expressions;
using Zoobee.Application.DTOs.Filters.Products.Base_Product;
using Zoobee.Application.Filtration.Base;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Domain.Enums;

namespace Zoobee.Infrastructure.Specifications.Base
{
	public class BaseProductSpecification<T> : BaseSpecification<T>
		where T : BaseProductEntity
	{
		public BaseProductSpecification(BaseProductFilterDto filter,
			bool paginate = false, int pageSize = 10, int pageNum = 1)
			: base(paginate, filter.OrderingType, pageSize, pageNum)
		{
			Criteria = PredicateBuilder.New<T>(true);
			if(filter.Price != null)
			{
				Criteria.And(ent =>
				ent.MinPrice <= filter.Price.Max
				&& ent.MinPrice >= filter.Price.Min);
			}
			if (filter.RatingRange != null)
			{
				Criteria.And(ent =>
				ent.Rating >= filter.RatingRange.Min
				&& ent.Rating <= filter.RatingRange.Max);
			}
			if(filter.FromCountries != null)
			{
				Includes.Add(e => e.CreatorCountry);
				Criteria.And(
					e => filter.FromCountries
					.Any(c => e.CreatorCountry.CountryName == c));
			}
			if(filter.FromBrands != null)
			{
				Includes.Add(e => e.Brand);
				Criteria.And(
					t => filter.FromBrands
					.Any(str => str == t.Brand.BrandName));
			}
			if (filter.ProductLineups != null)
			{
				Includes.Add(e => e.ProductLineup);
				Criteria.And(
					ent => filter.ProductLineups
					.Any(str => str == ent.Name));
			}
			if(filter.CreatorCompanies != null)
			{
				Includes.Add(e => e.CreatorCompany);
				Criteria.And(
					ent => filter.CreatorCompanies
					.Any(str => str == ent.CreatorCompany.CompanyName));
			}
			//TODO 
			//Переписать на отдельные предикаты вместо .ANY (см. Как в ToiletProductSpecification)
			if (filter.Tags != null)
			{
				Includes.Add(e => e.Tags);
				Criteria.And(
					ent => ent.Tags
					.Select(etag => etag.TagName)
					.Intersect(filter.Tags)
					.Any());
			}
			if(filter.PetKinds != null)
			{
				Includes.Add(e => e.PetKind);
				Criteria.And(
					ent => filter.PetKinds
					.Any(s => s == ent.PetKind.PetKindName));
			}
			//TODO
			//В будущем, добавить сортировку по нескольким полям
			//По дефолту по цене
			if (filter.OrderingType.Equals(OrderingType.OrderBy))
				OrderBy = T => T.MinPrice;
			else if(filter.OrderingType.Equals(OrderingType.OrderByDescending))
				OrderByDescending = T => T.MinPrice;
		}

	}
}
