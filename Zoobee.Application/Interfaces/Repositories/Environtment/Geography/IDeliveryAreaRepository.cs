using Zoobee.Application.Interfaces.Repositories.IRepositoryBase;
using Zoobee.Domain.DataEntities.Environment.Geography;

namespace Zoobee.Application.Interfaces.Repositories.Environtment.Geography
{
	public interface IDeliveryAreaRepository : IRepositoryBase<DeliveryAreaEntity>
	{
		public bool IsTemplateAreaWithNameExists(string templateAreaName);
		public bool IsCompanyAreaWithNameExists(string areaName, string companyName);
	}
}