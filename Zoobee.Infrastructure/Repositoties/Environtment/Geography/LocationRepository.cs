using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Environtment.Geography;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Environment.Geography;

namespace Zoobee.Infrastructure.Repositoties.Environtment.Geography
{
	public class LocationRepository : RepositoryBase, ILocationsRepository
	{
		public LocationRepository(ZoobeeAppDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<LocationEntity> GetAll() => dbContext.Locations;
		public async Task<OperationResult<Guid>> CreateAsync(LocationEntity newLocationEntity)
		{
			newLocationEntity.NormalizedCity = NormalizeString(newLocationEntity.City);
			newLocationEntity.NormalizedAddress = NormalizeString(newLocationEntity.Address);
			var res = await dbContext.Locations.AddAsync(newLocationEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.Locations.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}

		public OperationResult Delete(LocationEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.Locations.LocationNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}

		public LocationEntity Get(Guid Id) => dbContext.Locations.FirstOrDefault(e => e.Id == Id);
		/// <summary>
		///TODO При изменении через метод, не будет меняться нормализованыые данные. Надо как то исправить
		/// </summary>
		public OperationResult Update(LocationEntity productTypeToUpdate, Action<LocationEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.Locations.LocationNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id) => dbContext.Locations.Any(e => e.Id == Id);

		public LocationEntity GetByAddress(string city, string address)
			=> dbContext.Locations.FirstOrDefault(e => e.NormalizedCity == NormalizeString(city) && e.NormalizedAddress == NormalizeString(address));

		public LocationEntity GetByGeoPoint(GeoPoint geoPoint)
			=> dbContext.Locations.FirstOrDefault(e => e.GeoPoint == geoPoint);
	}
}