using FluentValidation.Results;
using Zoobee.Domain;

namespace Zoobee.Application.Interfaces.Services.EnvirontmentDataSeeding
{
	public interface IEnvirontmentDataSeedingService
	{
		public Task<List<OperationResult<ValidationResult>>> JsonSeedBrands(Stream json);
		public Task<List<OperationResult<ValidationResult>>> JsonSeedCreatorCountries(Stream json);
		public Task<List<OperationResult<ValidationResult>>> JsonSeedCreatorCompanies(Stream json);
		public Task<List<OperationResult<ValidationResult>>> JsonSeedProductLineups(Stream json);
		public Task<List<OperationResult<ValidationResult>>> JsonSeedSellerCompanies(Stream json);
		public Task<List<OperationResult<ValidationResult>>> JsonSeedZooStores(Stream json);

		public Task<List<OperationResult<ValidationResult>>> JsonSeedPetKinds(Stream json);
		public Task<List<OperationResult<ValidationResult>>> JsonSeedDeliveryAreas(Stream json);
	}
}
