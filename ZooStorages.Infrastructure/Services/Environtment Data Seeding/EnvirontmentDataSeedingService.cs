using Microsoft.Extensions.Localization;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environment;
using Zoobee.Core.Errors;
using Zoobee.Domain.Localization;
using System.Text.Json;
using FluentValidation.Results;
using FluentValidation;
using Zoobee.Domain.DataEntities.Environment.Creators;
using Zoobee.Domain;
using Zoobee.Application.DTOs.Environtment.Manufactures;
using System.Net;
using Zoobee.Domain.DataEntities.Environment.Manufactures;
using Zoobee.Domain.DataEntities.Environment.Geography;
using Zoobee.Application.DTOs.Environtment.Geography;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Application.DTOs.Environtment.Pets;
using Zoobee.Application.Interfaces.Services.EnvirontmentDataSeeding;
using Zoobee.Application.Interfaces.Services.GeoServices.GeoLocationService;
using Microsoft.Extensions.DependencyInjection;


namespace Zoobee.Infrastructure.Services.EnvirontmetnDataSeeding
{
    public class EnvirontmentDataSeedingService : IEnvirontmentDataSeedingService
    {
        public EnvirontmentDataSeedingService(IStringLocalizer<Validations> validLocalizer,
            IStringLocalizer<Errors> errorLocalizer,
            IEnvirontmentDataUnitOfWork envUOW,
			IGeoLocationService geoService,
            IServiceProvider serviceProvider)
        {
            ValidLocalizer = validLocalizer;
            ErrorLocalizer = errorLocalizer;
            EnvUOW = envUOW;
			geoLocationService = geoService;
            services = serviceProvider;
        }
		private IGeoLocationService geoLocationService { get; set; }
        private IStringLocalizer<Validations> ValidLocalizer { get; }
        private IStringLocalizer<Errors> ErrorLocalizer { get; }
        private IEnvirontmentDataUnitOfWork EnvUOW { get; }
        private IServiceProvider services { get; }

        /// <summary>
        /// Метод десериализует и валидирует из потока набор обьектов, после чего записывает их в бд.
        /// Для каждого полученного обьекта, метод вернет результат операции с информацией о валидации обьекта.
        /// </summary>
        /// <returns>
        /// Список результатов, по следующему плану:
        /// 1 - Валидация обьекта провалена	-> Error с информацией о ValidationResult
        /// 2 - Валидация успешна, запись не удалась -> Error без информации о ValidationResult (Returns = null)
        /// 3 - Валидация и запись успешны -> Success
        /// 4 - Обьекты не получены -> Пустой список
        /// </returns>
        public async Task<List<OperationResult<ValidationResult>>> JsonSeedBrands(Stream json)
        {
            List<OperationResult<ValidationResult>> operationResults = new List<OperationResult<ValidationResult>>();
            List<(ValidationResult validationResult, BrandDto brand)> brandsValues = await ValidatedDesirializationAsync<BrandDto>(json);
            if (!brandsValues.Any())
                return operationResults;
            foreach (var item in brandsValues)
            {
                if (item.validationResult.IsValid)
                {
                    var entity = new BrandEntity
                    {
                        BrandName = item.brand.BrandName,
                        Description = item.brand.Description
                    };
                    var res = await EnvUOW.ManufacturesUOWork.BrandsRepository.CreateAsync(entity);
                    if (res.Succeeded)
                    {
                        operationResults.Add(OperationResult<ValidationResult>.Success(item.validationResult));
                    }
                    else
                    {
                        operationResults.Add(OperationResult<ValidationResult>
                            .Error(res.Message, res.ErrCode));
                    }
                }
                else
                {
                    operationResults.Add(OperationResult<ValidationResult>
                        .Error(item.validationResult, ValidLocalizer["Validation.General.OneOrMoreValidationErrors"], HttpStatusCode.BadRequest));
                }
            }
			EnvUOW.ManufacturesUOWork.ZooStoresRepository.SaveChanges();
            return operationResults;
        }
        /// <summary>
        /// Метод десериализует и валидирует из потока набор обьектов, после чего записывает их в бд.
        /// Для каждого полученного обьекта, метод вернет результат операции с информацией о валидации обьекта.
        /// </summary>
        /// <returns>
        /// Список результатов, по следующему плану:
        /// 1 - Валидация обьекта провалена	-> Error с информацией о ValidationResult
        /// 2 - Валидация успешна, запись не удалась -> Error без информации о ValidationResult (Returns = null)
        /// 3 - Валидация и запись успешны -> Success
        /// 4 - Обьекты не получены -> Пустой список
        /// </returns>
        public async Task<List<OperationResult<ValidationResult>>> JsonSeedCreatorCompanies(Stream json)
        {
            List<OperationResult<ValidationResult>> operationResults = new List<OperationResult<ValidationResult>>();
            List<(ValidationResult validationResult, CreatorCompanyDto dto)> brandsValues = await ValidatedDesirializationAsync<CreatorCompanyDto>(json);
            if (!brandsValues.Any())
                return operationResults;
            foreach (var item in brandsValues)
            {
                if (item.validationResult.IsValid)
                {
                    var entity = new CreatorCompanyEntity
                    {
                        CompanyName = item.dto.CompanyName,
                    };
                    var res = await EnvUOW.ManufacturesUOWork.CreatorCompaniesRepository.CreateAsync(entity);
                    if (res.Succeeded)
                    {
                        operationResults.Add(OperationResult<ValidationResult>.Success(item.validationResult));
                    }
                    else
                    {
                        operationResults.Add(OperationResult<ValidationResult>
                            .Error(res.Message, res.ErrCode));
                    }
                }
                else
                {
                    operationResults.Add(OperationResult<ValidationResult>
                        .Error(item.validationResult, ValidLocalizer["Validation.General.OneOrMoreValidationErrors"], HttpStatusCode.BadRequest));
                }
            }
			EnvUOW.ManufacturesUOWork.ZooStoresRepository.SaveChanges();

			return operationResults;
        }
        /// <summary>
        /// Метод десериализует и валидирует из потока набор обьектов, после чего записывает их в бд.
        /// Для каждого полученного обьекта, метод вернет результат операции с информацией о валидации обьекта.
        /// </summary>
        /// <returns>
        /// Список результатов, по следующему плану:
        /// 1 - Валидация обьекта провалена	-> Error с информацией о ValidationResult
        /// 2 - Валидация успешна, запись не удалась -> Error без информации о ValidationResult (Returns = null)
        /// 3 - Валидация и запись успешны -> Success
        /// 4 - Обьекты не получены -> Пустой список
        /// </returns>
        public async Task<List<OperationResult<ValidationResult>>> JsonSeedCreatorCountries(Stream json)
        {
            List<OperationResult<ValidationResult>> operationResults = new List<OperationResult<ValidationResult>>();
            List<(ValidationResult validationResult, CreatorCountryDto dto)> brandsValues = await ValidatedDesirializationAsync<CreatorCountryDto>(json);
            if (!brandsValues.Any())
                return operationResults;
            foreach (var item in brandsValues)
            {
                if (item.validationResult.IsValid)
                {
                    var entity = new CreatorCountryEntity
                    {
                        CountryName = item.dto.CountryName,
                    };
                    var res = await EnvUOW.ManufacturesUOWork.CreatorCountryRepository.CreateAsync(entity);
                    if (res.Succeeded)
                    {
                        operationResults.Add(OperationResult<ValidationResult>.Success(item.validationResult));
                    }
                    else
                    {
                        operationResults.Add(OperationResult<ValidationResult>
                            .Error(res.Message, res.ErrCode));
                    }
                }
                else
                {
                    operationResults.Add(OperationResult<ValidationResult>
                        .Error(item.validationResult, ValidLocalizer["Validation.General.OneOrMoreValidationErrors"], HttpStatusCode.BadRequest));
                }
            }
			EnvUOW.ManufacturesUOWork.ZooStoresRepository.SaveChanges();

			return operationResults;
        }
        /// <summary>
        /// Метод десериализует и валидирует из потока набор обьектов, после чего записывает их в бд.
        /// Для каждого полученного обьекта, метод вернет результат операции с информацией о валидации обьекта.
        /// </summary>
        /// <returns>
        /// Список результатов, по следующему плану:
        /// 1 - Валидация обьекта провалена	-> Error с информацией о ValidationResult
        /// 2 - Валидация успешна, запись не удалась -> Error без информации о ValidationResult (Returns = null)
        /// 3 - Валидация и запись успешны -> Success
        /// 4 - Обьекты не получены -> Пустой список
        /// </returns>
        public async Task<List<OperationResult<ValidationResult>>> JsonSeedDeliveryAreas(Stream json)
        {
            List<OperationResult<ValidationResult>> operationResults = new List<OperationResult<ValidationResult>>();
            List<(ValidationResult validationResult, DeliveryAreaDto dto)> areasValues = await ValidatedDesirializationAsync<DeliveryAreaDto>(json);
            if (!areasValues.Any())
                return operationResults;
            foreach (var item in areasValues)
            {
                if (item.validationResult.IsValid)
                {
                    var seller = EnvUOW.ManufacturesUOWork.SellerCompanyRepository.Get(item.dto.SellerCompanyName);
                    if (seller != null)
                    {
                        var entity = new DeliveryAreaEntity
                        {
                            AreaName = item.dto.AreaName,
                            GeoArea = item.dto.GeoArea,
                            IsTemplate = item.dto.IsTemplate,
                            SellerCompany = seller,
                        };
                        var res = await EnvUOW.GeoUOWork.DeliveryAreaRepository.CreateAsync(entity);
                        if (res.Succeeded)
                        {
                            operationResults.Add(OperationResult<ValidationResult>.Success(item.validationResult));
                        }
                        else
                        {
                            operationResults.Add(OperationResult<ValidationResult>
                                .Error(res.Message, res.ErrCode));
                        }
                    }
                    else
                        operationResults.Add(OperationResult<ValidationResult>
                            .Error(ErrorLocalizer["Error.Seed.DeliveryAreas.SellerCompanyNotFound"], HttpStatusCode.InternalServerError));
                }
                else
                {
                    operationResults.Add(OperationResult<ValidationResult>
                        .Error(item.validationResult, ValidLocalizer["Validation.General.OneOrMoreValidationErrors"], HttpStatusCode.BadRequest));
                }
            }
			EnvUOW.ManufacturesUOWork.ZooStoresRepository.SaveChanges();

			return operationResults;
        }
        /// <summary>
        /// Метод десериализует и валидирует из потока набор обьектов, после чего записывает их в бд.
        /// Для каждого полученного обьекта, метод вернет результат операции с информацией о валидации обьекта.
        /// </summary>
        /// <returns>
        /// Список результатов, по следующему плану:
        /// 1 - Валидация обьекта провалена	-> Error с информацией о ValidationResult
        /// 2 - Валидация успешна, запись не удалась -> Error без информации о ValidationResult (Returns = null)
        /// 3 - Валидация и запись успешны -> Success
        /// 4 - Обьекты не получены -> Пустой список
        /// </returns>
        public async Task<List<OperationResult<ValidationResult>>> JsonSeedPetKinds(Stream json)
        {
            List<OperationResult<ValidationResult>> operationResults = new List<OperationResult<ValidationResult>>();
            List<(ValidationResult validationResult, PetKindDto dto)> Values = await ValidatedDesirializationAsync<PetKindDto>(json);
            if (!Values.Any())
                return operationResults;
            foreach (var item in Values)
            {
                if (item.validationResult.IsValid)
                {
                    var entity = new PetKindEntity
                    {
                        PetKindName = item.dto.PetKindName
                    };
                    var res = await EnvUOW.PetsDataUOWork.petKindsRepository.CreateAsync(entity);
                    if (res.Succeeded)
                    {
                        operationResults.Add(OperationResult<ValidationResult>.Success(item.validationResult));
                    }
                    else
                    {
                        operationResults.Add(OperationResult<ValidationResult>
                            .Error(res.Message, res.ErrCode));
                    }
                }
                else
                {
                    operationResults.Add(OperationResult<ValidationResult>
                        .Error(item.validationResult, ValidLocalizer["Validation.General.OneOrMoreValidationErrors"], HttpStatusCode.BadRequest));
                }
            }
			EnvUOW.ManufacturesUOWork.ZooStoresRepository.SaveChanges();

			return operationResults;
        }
        /// <summary>
        /// Метод десериализует и валидирует из потока набор обьектов, после чего записывает их в бд.
        /// Для каждого полученного обьекта, метод вернет результат операции с информацией о валидации обьекта.
        /// </summary>
        /// <returns>
        /// Список результатов, по следующему плану:
        /// 1 - Валидация обьекта провалена	-> Error с информацией о ValidationResult
        /// 2 - Валидация успешна, запись не удалась -> Error без информации о ValidationResult (Returns = null)
        /// 3 - Валидация и запись успешны -> Success
        /// 4 - Обьекты не получены -> Пустой список
        /// </returns>
        public async Task<List<OperationResult<ValidationResult>>> JsonSeedProductLineups(Stream json)
        {
            List<OperationResult<ValidationResult>> operationResults = new List<OperationResult<ValidationResult>>();
            List<(ValidationResult validationResult, ProductLineupDto dto)> Values = await ValidatedDesirializationAsync<ProductLineupDto>(json);
            if (!Values.Any())
                return operationResults;
            foreach (var item in Values)
            {
                if (item.validationResult.IsValid)
                {
                    var brand = EnvUOW.ManufacturesUOWork.BrandsRepository.Get(item.dto.BrandName);
                    if (brand != null)
                    {
                        var entity = new ProductLineupEntity
                        {
                            Brand = brand,
                            LineupDescription = item.dto.LineupDescription,
                            LineupName = item.dto.LineupName,
                        };
                        var res = await EnvUOW.ManufacturesUOWork.ProductLineupRepository.CreateAsync(entity);
                        if (res.Succeeded)
                        {
                            operationResults.Add(OperationResult<ValidationResult>.Success(item.validationResult));
                        }
                        else
                        {
                            operationResults.Add(OperationResult<ValidationResult>
                                .Error(res.Message, res.ErrCode));
                        }
                    }
                    else
                        operationResults.Add(OperationResult<ValidationResult>
                            .Error(ErrorLocalizer["Error.Seed.ProductLineups.BrandNotFound"], HttpStatusCode.InternalServerError));
                }
                else
                {
                    operationResults.Add(OperationResult<ValidationResult>
                        .Error(item.validationResult, ValidLocalizer["Validation.General.OneOrMoreValidationErrors"], HttpStatusCode.BadRequest));
                }
            }
			EnvUOW.ManufacturesUOWork.ZooStoresRepository.SaveChanges();

			return operationResults;
        }
        /// <summary>
        /// Метод десериализует и валидирует из потока набор обьектов, после чего записывает их в бд.
        /// Для каждого полученного обьекта, метод вернет результат операции с информацией о валидации обьекта.
        /// </summary>
        /// <returns>
        /// Список результатов, по следующему плану:
        /// 1 - Валидация обьекта провалена	-> Error с информацией о ValidationResult
        /// 2 - Валидация успешна, запись не удалась -> Error без информации о ValidationResult (Returns = null)
        /// 3 - Валидация и запись успешны -> Success
        /// 4 - Обьекты не получены -> Пустой список
        /// </returns>
        public async Task<List<OperationResult<ValidationResult>>> JsonSeedSellerCompanies(Stream json)
        {
            List<OperationResult<ValidationResult>> operationResults = new List<OperationResult<ValidationResult>>();
            List<(ValidationResult validationResult, SellerCompanyDto dto)> Values = await ValidatedDesirializationAsync<SellerCompanyDto>(json);
            if (!Values.Any())
                return operationResults;
            foreach (var item in Values)
            {
                if (item.validationResult.IsValid)
                {
                    var entity = new SellerCompanyEntity
                    {
                        CompanyName = item.dto.CompanyName
                    };
                    var res = await EnvUOW.ManufacturesUOWork.SellerCompanyRepository.CreateAsync(entity);
                    if (res.Succeeded)
                    {
                        operationResults.Add(OperationResult<ValidationResult>.Success(item.validationResult));
                    }
                    else
                    {
                        operationResults.Add(OperationResult<ValidationResult>
                            .Error(res.Message, res.ErrCode));
                    }
                }
                else
                {
                    operationResults.Add(OperationResult<ValidationResult>
                        .Error(item.validationResult, ValidLocalizer["Validation.General.OneOrMoreValidationErrors"], HttpStatusCode.BadRequest));
                }
            }
			EnvUOW.ManufacturesUOWork.ZooStoresRepository.SaveChanges();

			return operationResults;
        }
        /// <summary>
        /// Метод десериализует и валидирует из потока набор обьектов, после чего записывает их в бд.
        /// Для каждого полученного обьекта, метод вернет результат операции с информацией о валидации обьекта.
        /// </summary>
        /// <returns>
        /// Список результатов, по следующему плану:
        /// 1 - Валидация обьекта провалена	-> Error с информацией о ValidationResult
        /// 2 - Валидация успешна, запись не удалась -> Error без информации о ValidationResult (Returns = null)
        /// 3 - Валидация и запись успешны -> Success
        /// 4 - Обьекты не получены -> Пустой список
        /// </returns>
        public async Task<List<OperationResult<ValidationResult>>> JsonSeedZooStores(Stream json)
        {
            List<OperationResult<ValidationResult>> operationResults = new List<OperationResult<ValidationResult>>();
            List<(ValidationResult validationResult, ZooStoreDto dto)> Values = await ValidatedDesirializationAsync<ZooStoreDto>(json);
            if (!Values.Any())
                return operationResults;
            foreach (var item in Values)
            {
                if (item.validationResult.IsValid)
                {
                    var sellerCompany = EnvUOW.ManufacturesUOWork.SellerCompanyRepository.Get(item.dto.SellerCompanyName);
                    if (sellerCompany != null)
                    {
                        var res = await geoLocationService.CreateLocation(item.dto.Location);
                        if (res.Succeeded)
                        {
							var location = EnvUOW.GeoUOWork.LocationsRepository.Get(res.Returns);
							var entity = new ZooStoreEntity
                            {
                                ClosingTime = item.dto.ClosingTime,
                                OpeningTime = item.dto.OpeningTime,
                                Name = item.dto.Name,
                                StoreType = item.dto.StoreType,
                                SellerCompany = sellerCompany,
                                Location = location,
                            };
                            res = await EnvUOW.ManufacturesUOWork.ZooStoresRepository.CreateAsync(entity);
                            if (res.Succeeded)
                            {
                                operationResults.Add(OperationResult<ValidationResult>.Success(item.validationResult));
                                continue;
                            }
                        }
                        operationResults.Add(OperationResult<ValidationResult>
                            .Error(res.Message, res.ErrCode));
                    }
                    else
                        operationResults.Add(OperationResult<ValidationResult>
                            .Error(ErrorLocalizer["Error.Seed.ZooStores.SellerCompanyNotFound"], HttpStatusCode.InternalServerError));
                }
                else
                {
                    operationResults.Add(OperationResult<ValidationResult>
                        .Error(item.validationResult, ValidLocalizer["Validation.General.OneOrMoreValidationErrors"], HttpStatusCode.BadRequest));
                }
            }
			EnvUOW.ManufacturesUOWork.ZooStoresRepository.SaveChanges();

			return operationResults;
        }

        public async Task<List<(ValidationResult validRes, T value)>> ValidatedDesirializationAsync<T>(Stream json)
        {
            var jsonOpt = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
            };
			var result = new List<(ValidationResult, T)>();
			var values = await JsonSerializer.DeserializeAsync<List<T>>(json, jsonOpt);
			if (values == null)
				return result;

			using (var scope = services.CreateScope())
			{
				IValidator<T> validator = scope.ServiceProvider.GetRequiredService<IValidator<T>>();	
				foreach (var val in values)
				{
					IValidationContext context = new ValidationContext<T>(val);
					var res = await validator.ValidateAsync(context);
					result.Add((res, val));
				}
			}
            return result;
        }


    }
}