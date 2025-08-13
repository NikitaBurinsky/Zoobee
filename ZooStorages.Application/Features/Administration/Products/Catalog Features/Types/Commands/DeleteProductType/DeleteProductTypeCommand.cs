using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;

namespace ZooStorages.Application.Features.Catalog_Features.Types.Commands
{
	public class DeleteProductTypeCommand : IRequest<OperationResult>
	{
		public string ProductTypeName { get; set; }
	}

	public class DeleteProductTypeCommandHandler : IRequestHandler<DeleteProductTypeCommand, OperationResult>
	{
		IStringLocalizer<Errors> _localizer;
		IMapper _mapper;
		IUnitOfWork unitOfWork;

		public async Task<OperationResult> Handle(DeleteProductTypeCommand request, CancellationToken cancellationToken)
		{
			var type = await unitOfWork.ProductTypes.GetProductTypeAsync(request.ProductTypeName);
			if (type == null)
				return OperationResult.Error(_localizer["Error.ProductTypes.ProductTypeNotFound"], HttpStatusCode.NotFound);
			var res = await unitOfWork.ProductTypes.DeleteProductTypeAsync(type);
			if (res.Succeeded)
				await unitOfWork.SaveChangesAsync();
			return res;

		}
		public DeleteProductTypeCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_localizer = localizer;
			_mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

	}
}
