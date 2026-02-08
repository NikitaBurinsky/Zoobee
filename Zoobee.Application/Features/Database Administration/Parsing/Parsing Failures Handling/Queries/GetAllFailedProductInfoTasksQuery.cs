using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Zoobee.Application.Interfaces.Services.MappingService;
using Zoobee.Application.Interfaces.Services.System.Parsing;
using Zoobee.Application.Shared.DTOs.System.Parsing;
using Zoobee.Core.Errors;
using Zoobee.Domain;

namespace Zoobee.Application.Features.Database_Administration.Parsing.Parsing_Failures_Handling
{
	public class GetAllFailedProductInfoTasksQuery : IRequest<OperationResult<List<FailedToSaveParsedItemTaskDto>>>
	{
		public int page { get; set; }
		public int pageSize { get; set; }

	}
	public class GetAllUnresolvedFailedProductInfoTasksQueryHandler : IRequestHandler<GetAllFailedProductInfoTasksQuery, OperationResult<List<FailedToSaveParsedItemTaskDto>>>
	{
		IStringLocalizer<Errors> _localizer;
		IMappingService _mapper;
		IParsingFailuresService failuresService;

		public async Task<OperationResult<List<FailedToSaveParsedItemTaskDto>>> Handle(GetAllFailedProductInfoTasksQuery request, CancellationToken cancellationToken)
		{
			var res = (await failuresService.GetAllUnresolvedAsync(request.page, request.pageSize)).Where(t => t.FailedItemType == FailedItemType.Product);
			return OperationResult<List<FailedToSaveParsedItemTaskDto>>.Success(res.ToList());
		}
		public GetAllUnresolvedFailedProductInfoTasksQueryHandler(IStringLocalizer<Errors> localizer, IMappingService mapper,
			IParsingFailuresService failuresService)
		{
			this.failuresService = failuresService;
			_localizer = localizer;
			_mapper = mapper;
		}

	}
}
