using Zoobee.Application.Interfaces.Repositories.IRepositoryBase;
using Zoobee.Domain.DataEntities.Environment.Creators;

namespace Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures
{
	public interface ICreatorCountriesRepository : IRepositoryBase<CreatorCountryEntity>
	{
		public bool IsEntityExists(string countryName);
		public CreatorCountryEntity Get(string countryName);
	}
}