using Zoobee.Application.Interfaces.Repositories.IRepositoryBase;
using Zoobee.Domain.DataEntities.Environment.Manufactures;

namespace Zoobee.Application.Interfaces.Repositories.Environment.Manufactures
{
	public interface ISellerCompanyRepository : IRepositoryBase<SellerCompanyEntity>
	{
		public SellerCompanyEntity Get(string companyName);
		public bool IsEntityExists(string companyName);
	}
}