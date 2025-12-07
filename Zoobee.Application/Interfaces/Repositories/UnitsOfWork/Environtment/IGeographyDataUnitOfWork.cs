using Zoobee.Application.Interfaces.Repositories.Environtment.Geography;

namespace Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment
{
	public interface IGeographyDataUnitOfWork
	{
		public IDeliveryAreaRepository DeliveryAreaRepository { get; }
		public ILocationsRepository LocationsRepository { get; }

	}
}
