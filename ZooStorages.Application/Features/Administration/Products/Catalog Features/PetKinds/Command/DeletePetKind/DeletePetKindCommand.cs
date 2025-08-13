using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;

namespace ZooStorages.Application.Features.Catalog_Features.PetKinds.Command
{
	public class DeletePetKindCommand : IRequest<OperationResult>
	{
		public string TypeName { get; set; }
	}

	public class DeletePetKindCommandHandler : IRequestHandler<DeletePetKindCommand, OperationResult>
	{
		IStringLocalizer<Errors> _localizer;
		IMapper _mapper;
		IUnitOfWork unitOfWork;

		public async Task<OperationResult> Handle(DeletePetKindCommand request, CancellationToken cancellationToken)
		{
			PetKindEntity entity = await unitOfWork.PetKinds.GetPetKindAsync(request.TypeName);
			if (entity == null)
				return OperationResult.Error(_localizer["Error.PetKinds.PetKindNotFound"], HttpStatusCode.NotFound);
			OperationResult res = await unitOfWork.PetKinds.DeletePetKind(entity);
			if (res.Succeeded)
				await unitOfWork.SaveChangesAsync();
			return OperationResult.Success();
		}
		public DeletePetKindCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_localizer = localizer;
			_mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

	}
}
