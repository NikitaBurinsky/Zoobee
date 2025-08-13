using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Application.Interfaces.Services.ProductsAdminitrator;
using ZooStorages.Application.Models.Catalog.Product.Product;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStorages.Domain.DataEntities.Products.Components.Attributes;
using ZooStorages.Domain.Enums;
using ZooStorages.Domain.Localization;

namespace ZooStorages.Application.Features.Administration.Products.Products.Commands.CreateProductCommand
{
    public class CreateProductCommand : IRequest<OperationResult<Guid>>
    {
        public ProductDto newProduct { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, OperationResult<Guid>>
    {
        IStringLocalizer<Errors> _localizer;
        IMapper _mapper;
        IUnitOfWork unitOfWork;
        IProductsAdministratorService productsAdministratorService;

        public async Task<OperationResult<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var dto = request.newProduct;
            var petKind = await unitOfWork.PetKinds.GetPetKindAsync(dto.PetInfoAttributes.PetKind);
            var productType = await unitOfWork.ProductTypes.GetProductTypeAsync(dto.ProductType);
            var result = await productsAdministratorService.CreateProductAsync(dto);
            if (result.Succeeded)
                await unitOfWork.SaveChangesAsync();
            return result;
        }
        public CreateProductCommandHandler(IStringLocalizer<Errors> localizer,
            IMapper mapper, IUnitOfWork unitOfWork, IProductsAdministratorService products)
        {
            _localizer = localizer;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
            productsAdministratorService = products;
        }

    }
}
