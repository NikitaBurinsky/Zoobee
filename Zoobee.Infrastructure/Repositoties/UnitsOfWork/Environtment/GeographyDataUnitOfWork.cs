using Zoobee.Application.Interfaces.Repositories.Environtment.Geography;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment;

namespace Zoobee.Infrastructure.Repositoties.UnitsOfWork.Environtment
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
