using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;

namespace ZooStorages.Application.Features.Products.Types.Commands
{
    public class CreateProductTypeCommand : IRequest<OperationResult<Guid>>
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public string Category { get; set; }
    }

    public class CreateProductTypeCommandHandler : IRequestHandler<CreateProductTypeCommand, OperationResult<Guid>>
    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;

        public async Task<OperationResult<Guid>> Handle(CreateProductTypeCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.ProductTypes.GetProductCategoryAsync(request.Category);
            if (category == null)
                return OperationResult<Guid>.Error(_localizer["Error.ProductTypes.CategoryNotFound"], HttpStatusCode.NotFound);
            var newType = new ProductTypeEntity
            {
                Name = request.Name,
                Information = request.Info,
                Category = category
            };
            var res = await unitOfWork.ProductTypes.CreateProductTypeAsync(newType);
			if (res.Succeeded)
				await unitOfWork.SaveChangesAsync();
			return res;
        }
        public CreateProductTypeCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

    }
}
