using Zoobee.Application.Interfaces.Repositories.IRepositoryBase;
using Zoobee.Domain.DataEntities.Environment.Manufactures;

namespace Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures
{
	public interface IProductLineupRepository : IRepositoryBase<ProductLineupEntity>
	{
		bool IsEntityExists(string brandName, string lineupName);
		public ProductLineupEntity Get(string brandName, string lineupName);
	}
}