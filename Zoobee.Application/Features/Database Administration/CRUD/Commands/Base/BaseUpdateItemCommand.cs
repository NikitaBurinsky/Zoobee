using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Zoobee.Core.Errors;
using Zoobee.Domain;

namespace Zoobee.Application.Features.Database_Administration.CRUD.Commands.Base
{
	public abstract class BaseUpdateItemCommand<Type> : IRequest<OperationResult>
	{
	}
}
