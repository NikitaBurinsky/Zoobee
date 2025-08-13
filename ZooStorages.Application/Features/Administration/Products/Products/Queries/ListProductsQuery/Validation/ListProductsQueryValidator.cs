using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Features.Administration.Products.Products.Queries.ListProductsQuery;
using ZooStorages.Application.Validation;
using ZooStorages.Domain.Localization;

namespace ZooStorages.Application.Features.Administration.Products.Products.Queries.ListProductsQuery.Validation
{
    public class ListProductsQueryValidator : BaseDtoValidator<ListProductsQuery>
    {
        public ListProductsQueryValidator(
            IStringLocalizer<Validations> localizer) : base(localizer)
        {
            this.localizer = localizer;
            RuleFor(e => e.pageNum)
                .GreaterThan(0).WithMessage(ValidationMessage("PageNum.MustBeGreaterThan0"));
            RuleFor(e => e.pageSize)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage("PageSize.MustBePositive"));
        }
    }
}
