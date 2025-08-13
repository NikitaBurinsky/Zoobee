using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;

namespace ZooStorages.Application.Features.Catalog_Features.Categories.Commands
{
	public class DeleteProductCategoryCommand : IRequest<OperationResult>
	{
		public string CategoryName { get; set; }
	}

	public class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand, OperationResult>
	{
		IStringLocalizer<Errors> _localizer;
		IMapper _mapper;
		IUnitOfWork unitOfWork;
		public async Task<OperationResult> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
		{
			var entity = await unitOfWork.ProductTypes.GetProductCategoryAsync(request.CategoryName);
			if (entity == null)
				return OperationResult.Error(_localizer["Error.ProductCategories.CategoryNotFound"], HttpStatusCode.BadRequest);

			var res = await unitOfWork.ProductTypes.DeleteProductCategoryAsync(entity);
			
			if (res.Succeeded)
				await unitOfWork.SaveChangesAsync();
			return res;
		}
		public DeleteProductCategoryCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_localizer = localizer;
			_mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

	}
}
