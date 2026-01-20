using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Zoobee.Core.Errors;
using Zoobee.Domain;

namespace Zoobee.Application.Features.Database_Administration.Parsing.Parsing_Failures_Handling
{
	public class GetAllFailedProductInfoTasksQuery : IRequest<OperationResult>
	{
		public int page { get; set; }
		public int pageSize
		{
			get; set;
		}

		public class GetAllFailedProductInfoTasksQueryHandler : IRequestHandler<GetAllFailedProductInfoTasksQuery, OperationResult>
		{
			IStringLocalizer<Errors> _localizer;
			IMapper _mapper;

			public async Task<OperationResult<FailedParsedSaveTaskDto>> Handle(GetAllFailedProductInfoTasksQuery request, CancellationToken cancellationToken)
			{

			}
			public GetAllFailedProductInfoTasksQueryHandler(IStringLocalizer<Errors> localizer, IMapper mapper)
			{
				_localizer = localizer;
				_mapper = mapper;
			}

		}
	}
}
