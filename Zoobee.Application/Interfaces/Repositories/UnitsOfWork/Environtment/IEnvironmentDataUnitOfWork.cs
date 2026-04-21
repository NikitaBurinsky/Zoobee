namespace Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environment
{
	public interface IEnvironmentDataUnitOfWork
	{
		IGeographyDataUnitOfWork GeoUOWork { get; }
		IManufacturesUnitOfWork ManufacturesUOWork { get; }
		IPetsDataUnitOfWork PetsDataUOWork { get; }
	}
}
