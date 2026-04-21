using Zoobee.Application.Interfaces.Repositories.Environment.Pets;

namespace Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environment
{
	public interface IPetsDataUnitOfWork
	{
		IPetKindsRepository petKindsRepository { get; }
	}
}
