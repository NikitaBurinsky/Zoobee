using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Net;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core;
using ZooStorages.Core.Errors;
using ZooStorages.Infrastructure.Repositoties;
using ZooStores.Application.DtoTypes.Companies;
using ZooStores.Application.DtoTypes.EntityComponents;
using ZooStores.Application.DtoTypes.Stores;
namespace ZooStores.Infrastructure.Repositoties
{
	public class CompaniesRepository : RepositoryBase, ICompaniesRepository
    {
		public CompaniesRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{}
		public IQueryable<CompanyEntity> Companies => dbContext.Companies;
		
		public async Task<OperationResult<Guid>> CreateCompanyAsync(CompanyEntity newCompany)
		{
			if (IsCompanyExists(newCompany.Name)) 
				return OperationResult<Guid>.Error(localizer["Error.Companies.SimilarNameExists"], HttpStatusCode.BadRequest);
			CompanyEntity entity = new CompanyEntity
			{
				Name = newCompany.Name,
				ContactInfo = newCompany.ContactInfo,
				Description = newCompany.Description
			};
			var entry = dbContext.Companies.Add(entity);
			if (entry == null)
				return OperationResult<Guid>.Error(localizer["Error.Companies.UnknownWriteError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(entry.Entity.Id);
		}
		public async Task<OperationResult> UpdateCompanyAsync(CompanyEntity entityToUpdate, Action<CompanyEntity> action)
		{
			var entry = dbContext.Companies.Update(entityToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.Companies.UnknownReadError"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}
		public async Task<OperationResult> DeleteCompanyAsync(CompanyEntity newCompany)
		{
			if (IsCompanyExists(newCompany.Id))
			{
				foreach (var s in newCompany.ownedStores)
					dbContext.VetStores.Remove(s);
				foreach (var cl in newCompany.ownedClinics)
					dbContext.VetClinics.Remove(cl);
				dbContext.Companies.Remove(newCompany);
				return OperationResult.Success();
			}
			else
				return OperationResult.Error(localizer["Erros.Companies.CompanyNotFound"], HttpStatusCode.NotFound);
		}

		public async Task<CompanyEntity> GetCompanyAsync(Guid id) => await dbContext.Companies.FindAsync(id);
		public async Task<CompanyEntity> GetCompanyAsync(string companyName)
				=> await dbContext.Companies.FirstOrDefaultAsync(c => c.Name == companyName);

		Task ICompaniesRepository.LoadCompanyStoresAsync(CompanyEntity company) {
			dbContext.Entry(company).Collection(e => e.ownedClinics).Load();
			foreach (var clinic in company.ownedStores)
				dbContext.Entry(clinic).Reference(c => c.Location).Load();
			return Task.CompletedTask;
		}
		Task ICompaniesRepository.LoadCompanyClinicsAsync(CompanyEntity company) {
			dbContext.Entry(company).Collection(e => e.ownedClinics).Load();
			foreach (var clinic in company.ownedClinics)
				dbContext.Entry(clinic).Reference(c => c.location).Load();
			return Task.CompletedTask;
		}
		public bool IsCompanyExists(Guid id) => dbContext.Companies.Any(c => c.Id == id);
		public bool IsCompanyExists(string companyName) => dbContext.Companies.Any(c => c.Name == companyName);
		
	}

}
