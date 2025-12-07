using Zoobee.Application.Interfaces.Repositories.IRepositoryBase;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Application.Interfaces.Repositories.Environtment.Pets
{
	public interface IPetKindsRepository : IRepositoryBase<PetKindEntity>
	{
		public bool IsEntityExists(string petKindName);
		public PetKindEntity Get(string petKindName);
	}
}