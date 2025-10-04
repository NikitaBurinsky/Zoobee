using Microsoft.Extensions.Localization;
using System;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Catalog;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Catalog.Reviews;
using Zoobee.Domain.DataEntities.Environment.Geography;
using Zoobee.Domain.DataEntities.Environment.Manufactures;
using Zoobee.Infrastructure.Repositoties;

namespace Zoobee.Infrastructure.Repositoties.Environtment.Manufactures
{
	public class CreatorCompanyRepository : RepositoryBase, ICreatorCompaniesRepository
	{
		public CreatorCompanyRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<CreatorCompanyEntity> GetAll() => dbContext.CreatorCompanies;
		public async Task<OperationResult<Guid>> CreateAsync(CreatorCompanyEntity newCreatorCompanyEntity)
		{
			newCreatorCompanyEntity.NormalizedCompanyName = NormalizeString(newCreatorCompanyEntity.CompanyName);
			if (dbContext.CreatorCompanies.Any(e => e.NormalizedCompanyName == newCreatorCompanyEntity.NormalizedCompanyName))
				return OperationResult<Guid>.Error(localizer["Error.CretorCompanies.SimilarNameExists"], HttpStatusCode.InternalServerError);
			var res = await dbContext.CreatorCompanies.AddAsync(newCreatorCompanyEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.CreatorCompanies.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(CreatorCompanyEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.CreatorCompanies.CreatorCompanyNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public CreatorCompanyEntity Get(Guid Id) => dbContext.CreatorCompanies.FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(CreatorCompanyEntity productTypeToUpdate, Action<CreatorCompanyEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.CreatorCompanies.CreatorCompanyNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id)
			=> dbContext.CreatorCompanies.Any(e => e.Id == Id);

		public bool IsEntityExists(string companyName)
			=> dbContext.CreatorCompanies.Any(e => e.NormalizedCompanyName == NormalizeString(companyName));

		public CreatorCompanyEntity Get(string companyName)
			=> dbContext.CreatorCompanies.FirstOrDefault(e => e.NormalizedCompanyName == NormalizeString(companyName));
	}
}