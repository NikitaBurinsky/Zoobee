using MediatR;
using Zoobee.Domain;

namespace Zoobee.Application.Features.Database_Administration.CRUD.Commands.Base
{
	public abstract class BaseDeleteItemCommand<Type> : IRequest<OperationResult>
	{
	}
}
