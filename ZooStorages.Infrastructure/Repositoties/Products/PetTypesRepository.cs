using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories.Products;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Domain.DataEntities.Products;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStores.Infrastructure.Repositoties;

namespace ZooStorages.Infrastructure.Repositoties.Products
{
	public class PetKindsRepository : RepositoryBase, IPetKindsRepository
	{
		public PetKindsRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{}

		public IQueryable<PetKindEntity> PetKinds => dbContext.PetKinds;

		public async Task<OperationResult<Guid>> CreatePetKindAsync(PetKindEntity newProductType)
		{
			if (IsPetKindExists(newProductType.PetKindName))
				return OperationResult<Guid>.Error(localizer["Error.PetKinds.SimilarNameExists"], HttpStatusCode.BadRequest);
			var res = await dbContext.PetKinds.AddAsync(newProductType);
			return res != null ?
				OperationResult<Guid>.Success(res.Entity.Id) :
				OperationResult<Guid>.Error(localizer["Error.PetKinds.WriteDbError"], HttpStatusCode.InternalServerError);
		}
		public async Task<PetKindEntity> GetPetKindAsync(Guid Id)
			=> dbContext.PetKinds.FirstOrDefault(e => e.Id == Id);
		public async Task<PetKindEntity> GetPetKindAsync(string TypeName)
			=> dbContext.PetKinds.FirstOrDefault(e => e.PetKindName == TypeName);
		public bool IsPetKindExists(string TypeName)
			=> dbContext.PetKinds.Any(e => e.PetKindName == TypeName);
		public bool IsPetKindExists(Guid Id)
			=> dbContext.PetKinds.Any(e => e.Id == Id);
		public async Task<OperationResult> UpdateTypeAsync(PetKindEntity PetKindToUpdate, Action<PetKindEntity> action)
		{
			if (!IsPetKindExists(PetKindToUpdate.Id))
				return OperationResult.Error(localizer["Error.Products.ProductNotFound"], HttpStatusCode.NotFound);
			action(PetKindToUpdate);
			return OperationResult.Success();
		}
		public async Task<OperationResult> DeletePetKind(PetKindEntity type)
		{
			if(IsPetKindExists(type.Id))
			{
				var res = dbContext.Remove(type);
				return OperationResult.Success();
			}
			return OperationResult.Error(localizer["Error.PetKinds.PetKindNotFound"], HttpStatusCode.NotFound);
		}
	}
}
