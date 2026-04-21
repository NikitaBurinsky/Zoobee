using Zoobee.Application.Interfaces.Repositories.Environment.Geography;

namespace Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environment
{
	public interface IGeographyDataUnitOfWork
	{
		public IDeliveryAreaRepository DeliveryAreaRepository { get; }
		public ILocationsRepository LocationsRepository { get; }

	}
}
