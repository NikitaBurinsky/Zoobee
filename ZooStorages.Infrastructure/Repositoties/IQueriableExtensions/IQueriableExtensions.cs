using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Zoobee.Application.Interfaces.Repositories;
using Zoobee.Core;
using Zoobee.Core.Errors;

namespace Zoobee.Infrastructure.Repositoties.IQueriableExtensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(
            this IQueryable<T> source,
            int pageNumber,
            int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentException("Номер страницы должен быть >= 1", nameof(pageNumber));
            if (pageSize < 1)
                throw new ArgumentException("Размер страницы должен быть >= 1", nameof(pageSize));
            return source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
