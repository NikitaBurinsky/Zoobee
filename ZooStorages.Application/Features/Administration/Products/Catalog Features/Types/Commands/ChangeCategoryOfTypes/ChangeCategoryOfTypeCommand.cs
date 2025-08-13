using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;

namespace ZooStorages.Application.Features.Catalog_Features.Types.Commands
{
	public class ChangeCategoryOfTypeCommand : IRequest<OperationResult>
	{
		public string TypeName { get; set; }
		public string NewCategory { get; set; }
	}

	public class ChangeCategoryOfTypeCommandHandler : IRequestHandler<ChangeCategoryOfTypeCommand, OperationResult>
	{
		IStringLocalizer<Errors> _localizer;
		IMapper _mapper;
		IUnitOfWork unitOfWork;

		public async Task<OperationResult> Handle(ChangeCategoryOfTypeCommand request, CancellationToken cancellationToken)
		{
			var type = await unitOfWork.ProductTypes.GetProductTypeAsync(request.TypeName);
			var category = await unitOfWork.ProductTypes.GetProductCategoryAsync(request.NewCategory);
			if (type == null)
				return OperationResult.Error(_localizer["Error.ProductTypes.ProductTypeNotFound"], HttpStatusCode.NotFound);
			if(category == null)
				return OperationResult.Error(_localizer["Error.ProductTypes.ProductCategoryNotFound"], HttpStatusCode.NotFound);
			if (category.CategoryName == request.NewCategory)
				return OperationResult.Success();
			var res = await unitOfWork.ProductTypes.UpdateTypeAsync(type, e => e.Category = category);
			await unitOfWork.SaveChangesAsync();
			return res;
		}

		public ChangeCategoryOfTypeCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_localizer = localizer;
			_mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

	}
}
