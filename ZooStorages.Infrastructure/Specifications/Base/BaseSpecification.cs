using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Application.Interfaces.Repositories.IQueriableExtensions;
using Zoobee.Domain.Enums;

namespace Zoobee.Application.Filtration.Base
{
	public abstract class BaseSpecification<T> where T : BaseEntity
    {
		public Expression<Func<T, bool>> Criteria { get; set; }
		public virtual List<Expression<Func<T, object>>> Includes { get; private set; }
			= new List<Expression<Func<T, object>>>();
        public virtual Expression<Func<T, object>>? OrderBy { get; protected set; }
        public virtual Expression<Func<T, object>>? OrderByDescending { get; protected set; }
		public bool Paginate { get; set; } = true;
		public int PageSize { get; set; }
		/// <summary>
		/// Начиная с 1
		/// </summary>
		public int PageNum { get; set; }
		public OrderingType OrderType { get; private set; }

		protected BaseSpecification(bool paginate, OrderingType order,
			int pagesize, int pagenum)
		{
			Paginate = paginate;
			PageSize = pagesize;
			PageNum = pagenum;
			OrderType = order;
		}

		public void SetOrderingBy(Expression<Func<T, object>> orderByExpr)
		{
			OrderType = OrderingType.OrderBy;
			OrderBy = orderByExpr;
		}
		public void SetOrderingByDescending(Expression<Func<T,object>> orderByDescExpr)
		{
			OrderType = OrderingType.OrderByDescending;
			OrderByDescending = orderByDescExpr;
		}


	}
	public static class SpecificationApplyerExtension
	{
		public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, BaseSpecification<T> specification) 
			where T : BaseEntity
		{
			query = specification.Includes.Aggregate(query,
				(current, include) => current.Include(include));
			query = query.Where(specification.Criteria);
			if (specification.Paginate)
				query = query.Paginate(specification.PageNum, specification.PageSize);
			if (specification.OrderType.Equals(OrderingType.OrderBy))
			{
				if (specification.OrderBy == null)
					throw new ArgumentNullException("TODO Ошибка. Не установлена функция сортировки 5264");
				query.OrderBy(specification.OrderBy);
			}
			if (specification.OrderType.Equals(OrderingType.OrderByDescending))
			{
				if (specification.OrderBy == null)
					throw new ArgumentNullException("TODO Ошибка. Не установлена функция сортировки 4637");
				query.OrderByDescending(specification.OrderByDescending);
			}
			return query;
		}
	}
}
