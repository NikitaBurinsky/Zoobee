using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain;

namespace Zoobee.Application.Interfaces.Services.MappingService
{
	public interface IMappingService
	{
		/// <summary>
		/// Map Dto to Entity
		/// </summary>
		/// <typeparam name="Dto"></typeparam>
		/// <typeparam name="Entity"></typeparam>
		/// <param name="from"></param>
		/// <returns></returns>
		public OperationResult<Entity> Map<Dto, Entity>(Dto from)
			where Entity : BaseEntity;
		/// <summary>
		/// Map Entity to Dto
		/// </summary>
		/// <typeparam name="Dto"></typeparam>
		/// <typeparam name="Entity"></typeparam>
		/// <param name="to"></param>
		/// <returns></returns>
		public OperationResult<Dto> RevMap<Dto, Entity>(Entity from)
			where Entity : BaseEntity;

	}
}
