using Zoobee.Application.Interfaces.Repositories.IRepositoryBase;
using Zoobee.Domain.DataEntities.Environment.Manufactures;

namespace Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures
{
	public interface ICreatorCompaniesRepository : IRepositoryBase<CreatorCompanyEntity>
	{
		public bool IsEntityExists(string companyName);
		public CreatorCompanyEntity Get(string companyName);
	}
}