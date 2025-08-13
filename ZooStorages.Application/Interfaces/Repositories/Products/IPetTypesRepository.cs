using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Core;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStorages.Domain.DataEntities.Products;
using ZooStores.Application.DtoTypes.Products.Categories;

namespace ZooStorages.Application.Interfaces.Repositories.Products
{
	public interface IPetKindsRepository : IRepositoryBase
	{
		IQueryable<PetKindEntity> PetKinds { get; }
		public Task<OperationResult<Guid>> CreatePetKindAsync(PetKindEntity newProductType);
		public Task<PetKindEntity> GetPetKindAsync(Guid Id);
		public Task<PetKindEntity> GetPetKindAsync(string TypeName);
		public Task<OperationResult> DeletePetKind(PetKindEntity type);
		public bool IsPetKindExists(string TypeName);
		public bool IsPetKindExists(Guid Id);
		public Task<OperationResult> UpdateTypeAsync(PetKindEntity productTypeToUpdate, Action<PetKindEntity> action);
	}
}
