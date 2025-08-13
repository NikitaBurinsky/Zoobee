using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;
using ZooStores.Application.DtoTypes.Products.Categories;

namespace ZooStorages.Application.Features.Products.Categories.Commands
{
    public class CreateProductCategoryCommand : IRequest<OperationResult<Guid>>
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }

    public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, OperationResult<Guid>>
    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;

        public async Task<OperationResult<Guid>> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var newCategory = new ProductCategoryEntity
            {
                CategoryName = request.CategoryName,
                Description = request.Description,
            };
            var res = await unitOfWork.ProductTypes.CreateProductCategoryAsync(newCategory);
			if (res.Succeeded)
				await unitOfWork.SaveChangesAsync();
			return res;
        }
        public CreateProductCategoryCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

    }
}
