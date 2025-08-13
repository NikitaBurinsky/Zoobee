using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;

namespace ZooStorages.Application.Features.Catalog_Features.PetKinds
{
	public class CreatePetKindCommand : IRequest<OperationResult<Guid>>
	{
		public string NewTypeName { get; set; }
	}

	public class CreatePetKindCommandHandler : IRequestHandler<CreatePetKindCommand, OperationResult<Guid>>
	{
		IStringLocalizer<Errors> _localizer;
		IMapper _mapper;
		IUnitOfWork unitOfWork;
		
		public async Task<OperationResult<Guid>> Handle(CreatePetKindCommand request, CancellationToken cancellationToken)
		{
			var newEntity = new PetKindEntity { PetKindName = request.NewTypeName };
			var res = await unitOfWork.PetKinds.CreatePetKindAsync(newEntity);
			if (res.Succeeded)
				await unitOfWork.SaveChangesAsync();
			return res;
		}
		public CreatePetKindCommandHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_localizer = localizer;
			_mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

	}
}
