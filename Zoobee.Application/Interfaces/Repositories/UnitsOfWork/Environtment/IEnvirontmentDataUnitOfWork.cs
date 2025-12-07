namespace Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment
{
	public interface IEnvirontmentDataUnitOfWork
	{
		IGeographyDataUnitOfWork GeoUOWork { get; }
		IManufacturesUnitOfWork ManufacturesUOWork { get; }
		IPetsDataUnitOfWork PetsDataUOWork { get; }
	}
}
