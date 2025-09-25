using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.Interfaces.Repositories.Environtment.Pets;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment;

namespace Zoobee.Infrastructure.Repositoties.UnitsOfWork.Environtment
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
