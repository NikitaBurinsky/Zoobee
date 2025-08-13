using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;

namespace ZooStorages.Application.Features.Catalog_Features.PetKinds.Command
{
	public class RenamePetKindCommand : IRequest<OperationResult>
	{
		public Guid Id { get; set; }
		public string newName { get; set; }
	}

	public class RenamePetKindCommandHandler : IRequestHandler<RenamePetKindCommand, OperationResult>
	{
		IStringLocalizer<Errors> localizer;
		IMapper _mapper;
		IUnitOfWork unitOfWork;

		public async Task<OperationResult> Handle(RenamePetKindCommand request, CancellationToken cancellationToken)
		{
			if (!unitOfWork.PetKinds.IsPetKindExists(request.newName))
			{
				var entity = await unitOfWork.PetKinds.GetPetKindAsync(request.Id);
				if (entity != null)
				{
					var res = await unitOfWork.PetKinds.UpdateTypeAsync(entity, 
						e => { e.PetKindName = e.PetKindName; });
					await unitOfWork.SaveChangesAsync();
					return res;
				}
				return OperationResult.Error(localizer["Error.PetKinds.PetKindNotFound"], HttpStatusCode.NotFound);
			}
			else
				return OperationResult.Error(localizer["Error.PetKinds.SimilarNameExists"], HttpStatusCode.BadRequest);

		}
		public RenamePetKindCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			this.localizer = localizer;
			_mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

	}
}
