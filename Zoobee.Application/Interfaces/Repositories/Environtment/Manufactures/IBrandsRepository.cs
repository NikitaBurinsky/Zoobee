using Zoobee.Application.Interfaces.Repositories.IRepositoryBase;
using Zoobee.Domain.DataEntities.Environment.Creators;

namespace Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures
{
	public interface IBrandsRepository : IRepositoryBase<BrandEntity>
	{
		public BrandEntity Get(string brandName);
		public bool IsEntityExists(string brandName);
	}
}