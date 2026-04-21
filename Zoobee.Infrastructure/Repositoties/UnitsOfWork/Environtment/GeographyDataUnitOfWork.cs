using Zoobee.Application.Interfaces.Repositories.Environment.Geography;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environment;

namespace Zoobee.Infrastructure.Repositoties.UnitsOfWork.Environment
{
	public class GeographyDataUnitOfWork : IGeographyDataUnitOfWork
	{
		public GeographyDataUnitOfWork(IDeliveryAreaRepository deliveryAreaRepository, ILocationsRepository locationsRepository)
		{
			DeliveryAreaRepository = deliveryAreaRepository;
			LocationsRepository = locationsRepository;
		}

		public IDeliveryAreaRepository DeliveryAreaRepository { get; }

		public ILocationsRepository LocationsRepository { get; }
	}
}
