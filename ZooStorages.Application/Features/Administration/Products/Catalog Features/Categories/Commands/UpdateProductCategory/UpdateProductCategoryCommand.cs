using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStores.Application.DtoTypes.Products.Categories;

namespace ZooStorages.Application.Features.Catalog_Features.Categories.Commands
{
	public class UpdateProductCategoryCommand : IRequest<OperationResult>
	{
		public string oldName { get; set; }
		public string newName { get; set; }
		public string newDescription { get; set; }
	}

	public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand, OperationResult>
	{
		IStringLocalizer<Errors> _localizer;
		IMapper _mapper;
		IUnitOfWork unitOfWork;

		public async Task<OperationResult> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
		{
			var entity = await unitOfWork.ProductTypes.GetProductCategoryAsync(request.oldName);
			if (entity == null)
				return OperationResult.Error("Error.ProductCategories.ProductCategoryNotFound", HttpStatusCode.NotFound);
			if(unitOfWork.ProductTypes.IsCategoryExists(request.newName))
				return OperationResult.Error("Error.ProductCategories.SimilarNameExists", HttpStatusCode.BadRequest);
			var res = await unitOfWork.ProductTypes.UpdateCategoryAsync(entity, e
				=>
			{
				e.CategoryName = request.newName;
				e.Description = request.newDescription;
			});
			await unitOfWork.SaveChangesAsync();
			return res;
		}
		public UpdateProductCategoryCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_localizer = localizer;
			_mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

	}
}
