using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;

namespace ZooStorages.Application.Features.Catalog_Features.Types.Commands
{
	public class ChangeProductTypeNameCommand : IRequest<OperationResult>
	{
		public string typeName { get; set; }
		public string newName { get; set; }
	}

	public class ChangeProductTypeNameCommandHandler : IRequestHandler<ChangeProductTypeNameCommand, OperationResult>
	{
		IStringLocalizer<Errors> _localizer;
		IMapper _mapper;
		IUnitOfWork unitOfWork;

		public async Task<OperationResult> Handle(ChangeProductTypeNameCommand request, CancellationToken cancellationToken)
		{
			var entity = await unitOfWork.ProductTypes.GetProductTypeAsync(request.typeName);
			if (entity == null)
				return OperationResult.Error(_localizer["Error.ProductTypes.ProductTypeNotFound"], HttpStatusCode.NotFound);
			if (request.typeName != request.newName)
			{
				if (!unitOfWork.ProductTypes.IsTypeExists(request.newName))
				{
					var res = await unitOfWork.ProductTypes.UpdateTypeAsync(entity, e => { e.Name = request.newName; });
					await unitOfWork.SaveChangesAsync();
					return res;
				}
				else
					return OperationResult.Error(_localizer["Error.ProductTypes.SimilarNameExists"], HttpStatusCode.BadRequest);
			}
			else
				return OperationResult.Success();
		}
		public ChangeProductTypeNameCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_localizer = localizer;
			_mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

	}
}
