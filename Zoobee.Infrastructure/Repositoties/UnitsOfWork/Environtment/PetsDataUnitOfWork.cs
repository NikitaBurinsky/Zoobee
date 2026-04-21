using Zoobee.Application.Interfaces.Repositories.Environment.Pets;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environment;

namespace Zoobee.Infrastructure.Repositoties.UnitsOfWork.Environment
{
	public class PetsDataUnitOfWork : IPetsDataUnitOfWork
	{
		public PetsDataUnitOfWork(IPetKindsRepository petKindsRepository)
		{
			this.petKindsRepository = petKindsRepository;
		}

		public IPetKindsRepository petKindsRepository { get; }
	}
}
