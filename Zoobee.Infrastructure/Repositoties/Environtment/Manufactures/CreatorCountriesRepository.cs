using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Environment.Creators;

namespace Zoobee.Infrastructure.Repositoties.Environtment.Manufactures
{
	public class CreatorCountriesRepository : RepositoryBase, ICreatorCountriesRepository
	{
		public CreatorCountriesRepository(ZoobeeAppDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<CreatorCountryEntity> GetAll() => dbContext.CreatorCountries;
		public async Task<OperationResult<Guid>> CreateAsync(CreatorCountryEntity newCreatorCountryEntity)
		{
			newCreatorCountryEntity.NormalizedCountryName = NormalizeString(newCreatorCountryEntity.CountryName);
			var res = await dbContext.CreatorCountries.AddAsync(newCreatorCountryEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.CreatorCountries.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(CreatorCountryEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.CreatorCountries.CreatorCountryNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public CreatorCountryEntity Get(Guid Id) => dbContext.CreatorCountries.FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(CreatorCountryEntity productTypeToUpdate, Action<CreatorCountryEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.CreatorCountries.CreatorCountryNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id)
			=> dbContext.CreatorCountries.Any(e => e.Id == Id);

		public bool IsEntityExists(string countryName)
			=> dbContext.CreatorCountries.Any(e => e.NormalizedCountryName == NormalizeString(countryName));

		public CreatorCountryEntity Get(string countryName)
			=> dbContext.CreatorCountries.FirstOrDefault(e => e.NormalizedCountryName == NormalizeString(countryName));
	}
}