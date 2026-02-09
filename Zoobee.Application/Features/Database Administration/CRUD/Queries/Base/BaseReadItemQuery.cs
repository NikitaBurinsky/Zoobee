using MediatR;
using Zoobee.Domain;

namespace Zoobee.Application.Features.Database_Administration.CRUD.Queries.Base
{
	public abstract class BaseReadItemQuery<Type> : IRequest<OperationResult>
	{

	}
}
