using Zoobee.Application.Interfaces.Repositories.Environtment.Pets;

namespace Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment
{
	public interface IPetsDataUnitOfWork
	{
		IPetKindsRepository petKindsRepository { get; }
	}
}
