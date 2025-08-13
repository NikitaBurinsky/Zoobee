using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;

namespace ZooStorages.Application.Features.Catalog_Features.Types.Commands
{
	public class ChangeProductTypeInformation : IRequest<OperationResult>
	{
		public string typeName { get; set; }
		public string newInfo { get; set; }
	}

	public class ChangeProductTypeInformationHandler : IRequestHandler<ChangeProductTypeInformation, OperationResult>
	{
		IStringLocalizer<Errors> _localizer;
		IMapper _mapper;
		IUnitOfWork unitOfWork;

		public async Task<OperationResult> Handle(ChangeProductTypeInformation request, CancellationToken cancellationToken)
		{
			var entity = await unitOfWork.ProductTypes.GetProductTypeAsync(request.typeName);
			if (entity == null)
				return OperationResult.Error(_localizer["Error.ProductTypes.ProductTypeNotFound"], HttpStatusCode.NotFound);
			var res = await unitOfWork.ProductTypes.UpdateTypeAsync(entity, e => { e.Information = request.newInfo; });
			await unitOfWork.SaveChangesAsync();
			return res;
		}
		public ChangeProductTypeInformationHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_localizer = localizer;
			_mapper = mapper;
			this.unitOfWork = unitOfWork;
		}
	}
}

 