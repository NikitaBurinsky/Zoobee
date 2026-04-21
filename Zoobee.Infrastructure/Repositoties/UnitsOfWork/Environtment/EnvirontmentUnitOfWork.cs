using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environment;

namespace Zoobee.Infrastructure.Repositoties.UnitsOfWork.Environment
{
	internal class EnvironmentDataUnitOfWork : IEnvironmentDataUnitOfWork
	{
		public EnvironmentDataUnitOfWork(IGeographyDataUnitOfWork geoUOWork,
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
