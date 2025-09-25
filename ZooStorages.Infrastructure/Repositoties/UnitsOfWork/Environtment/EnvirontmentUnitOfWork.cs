using Zoobee.Application.Interfaces.Repositories.UnitsOfWork;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environment;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment;

namespace Zoobee.Infrastructure.Repositoties.UnitsOfWork.Environtment
{
	internal class EnvirontmentDataUnitOfWork : IEnvirontmentDataUnitOfWork
	{
		public EnvirontmentDataUnitOfWork(IGeographyDataUnitOfWork geoUOWork, 
			IManufacturesUnitOfWork manufacturesUOWork,
			IPetsDataUnitOfWork petsDataUnitOfWork)
		{
			GeoUOWork = geoUOWork;
			ManufacturesUOWork = manufacturesUOWork;
			PetsDataUOWork = petsDataUnitOfWork;
		}

		public IGeographyDataUnitOfWork GeoUOWork { get; }
		public IManufacturesUnitOfWork ManufacturesUOWork { get; }
		public IPetsDataUnitOfWork PetsDataUOWork { get; set; }
	}
}
