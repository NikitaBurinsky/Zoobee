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
using Zoobee.Infrastructure.Repositoties;

namespace Zoobee.Infrastructure.Repositoties.Environtment.Manufactures
{
	public class SellerCompaniesRepository : RepositoryBase, ISellerCompanyRepository
	{
		public SellerCompaniesRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<SellerCompanyEntity> GetAll() => dbContext.SellerCompanies;
		public async Task<OperationResult<Guid>> CreateAsync(SellerCompanyEntity newSellerCompanyEntity)
		{
			newSellerCompanyEntity.NormalizedCompanyName = NormalizeString(newSellerCompanyEntity.CompanyName);
			var res = await dbContext.SellerCompanies.AddAsync(newSellerCompanyEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.SellerCompanies.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(SellerCompanyEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.SellerCompanies.SellerCompanyNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public SellerCompanyEntity Get(Guid Id) => dbContext.SellerCompanies.FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(SellerCompanyEntity productTypeToUpdate, Action<SellerCompanyEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.SellerCompanies.SellerCompanyNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id) => dbContext.SellerCompanies.Any(e => e.Id == Id);

		public SellerCompanyEntity Get(string companyName)
			=> dbContext.SellerCompanies.FirstOrDefault(e => e.NormalizedCompanyName == NormalizeString(companyName));

		public bool IsEntityExists(string companyName)
			=> dbContext.SellerCompanies.Any(e => e.NormalizedCompanyName == NormalizeString(companyName));
			
	}
}