using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Environtment.Pets;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.Repositoties.Environtment.Pets
{
	public class PetKindsRepository : RepositoryBase, IPetKindsRepository
	{
		public PetKindsRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<PetKindEntity> GetAll() => dbContext.PetKinds;
		public async Task<OperationResult<Guid>> CreateAsync(PetKindEntity newPetKindEntity)
		{
			newPetKindEntity.NormalizedPetKindName = NormalizeString(newPetKindEntity.PetKindName);
			var res = await dbContext.PetKinds.AddAsync(newPetKindEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.PetKinds.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(PetKindEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.PetKinds.PetKindNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public PetKindEntity Get(Guid Id) => dbContext.PetKinds.FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(PetKindEntity productTypeToUpdate, Action<PetKindEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.PetKinds.PetKindNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}
		public bool IsEntityExists(Guid Id) => dbContext.PetKinds.Any(e => e.Id == Id);

		public bool IsEntityExists(string petKindName) => 
			dbContext.PetKinds.Any(e => e.NormalizedPetKindName == NormalizeString(petKindName));
	}
}