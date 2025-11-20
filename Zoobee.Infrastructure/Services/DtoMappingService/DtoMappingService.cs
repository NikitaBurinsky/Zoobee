using Zoobee.Application.DtoTypes.Base;
using Zoobee.Application.Interfaces.Services;
using Zoobee.Application.Interfaces.Services.Products.ProductsMapperService.Mapping_Profiles;
using Zoobee.Domain;

namespace Zoobee.Infrastructure.Services.DtoMappingService
{
	public class DtoMappingService : IMappingService

	{
		IServiceProvider serviceProvider;
		public DtoMappingService(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public OperationResult<Entity> Map<Dto, Entity>(Dto from) where Entity : BaseEntity
		{
			IBaseMappingProfile<Dto, Entity> mappingProfile = (IBaseMappingProfile<Dto, Entity>)serviceProvider.GetService(typeof(IBaseMappingProfile<Dto, Entity>));
			if (mappingProfile == null)
				throw new ArgumentNullException("TODO MappingProfile Was Not Found");
			return mappingProfile.Map(from);
		}

		public OperationResult<Dto> RevMap<Dto, Entity>(Entity to) where Entity : BaseEntity
		{
			IBaseMappingProfile<Dto, Entity> mappingProfile = (IBaseMappingProfile<Dto, Entity>)serviceProvider.GetService(typeof(IBaseMappingProfile<Dto, Entity>));
			if (mappingProfile == null)
				throw new ArgumentNullException("TODO MappingProfile Was Not Found");
			return mappingProfile.RevMap(to);
		}
	}
}
