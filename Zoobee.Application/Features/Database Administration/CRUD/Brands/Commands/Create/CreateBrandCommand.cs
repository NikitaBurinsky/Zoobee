using MediatR;
using Microsoft.Extensions.Localization;
using Zoobee.Application.Interfaces.Repositories.IRepositoryBase;
using Zoobee.Application.Interfaces.Services.MappingService;
using Zoobee.Application.Shared.DTOs.Environment.Manufactures;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Environment.Creators;

namespace Zoobee.Application.Features.Database_Administration.CRUD.Brands.Commands.Create
{
	public class CreateBrandCommand : IRequest<OperationResult<Guid>>
	{
		public BrandDto newBrand { get; set; }
	}

	public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, OperationResult<Guid>>
	{
		IStringLocalizer<Errors> _localizer;
		IMappingService _mapper;
		IRepositoryBase<BrandEntity> repository;

		public async Task<OperationResult<Guid>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
		{
			var res = _mapper.Map<BrandDto, BrandEntity>(request.newBrand);
			if(res.Failed)
				return OperationResult<Guid>.Error(res);
			var createRes = await repository.CreateAsync(res.Returns);
			return createRes;
		}
	}
}
