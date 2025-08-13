using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;
using System.Net;
using ZooStorages.Application.Features.Products.Catalog_Features.PetTypes.Queries.ListPetTypes;

namespace ZooStorages.Application.Features.Catalog_Features.PetKinds.Queries
{
	public class ListPetKindsQuery : IRequest<List<ListPetKindDto>>
	{}

	public class ListPetKindsQueryHandler : IRequestHandler<ListPetKindsQuery, List<ListPetKindDto>>
	{
		IStringLocalizer<Errors> _localizer;
		IMapper _mapper;
		IUnitOfWork unitOfWork;

		public async Task<List<ListPetKindDto>> Handle(ListPetKindsQuery request, CancellationToken cancellationToken)
		{
			return unitOfWork.PetKinds.PetKinds.Select(e => new ListPetKindDto { PetKindName = e.PetKindName }).ToList();
		}
		public ListPetKindsQueryHandler(IStringLocalizer<Errors> localizer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_localizer = localizer;
			_mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

	}
}
