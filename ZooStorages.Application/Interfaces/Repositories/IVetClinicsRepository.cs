using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Core;
using ZooStores.Application.DtoTypes.Clinics;
using ZooStores.Application.DtoTypes.Companies;

namespace ZooStorages.Application.Interfaces.Repositories
{
	public interface IVetClinicsRepository : IRepositoryBase
	{
		IQueryable<VetClinicEntity> VetClinics { get; }
		public Task<OperationResult<Guid>> CreateClinicAsync(VetClinicEntity newVetClinic);
		public Task<OperationResult> UpdateClinicAsync(VetClinicEntity updatedVetClinic);
		public Task<VetClinicEntity> GetByIdAsync(Guid id);
		public Task<OperationResult> DeleteClinicAsync(VetClinicEntity newVetClinic);
	}
}
