using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using Zoobee.Application.DTOs.Mapping_Profiles.Products;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Application.Interfaces.Repositories.Catalog;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment;
using Zoobee.Application.Interfaces.Services.Products.ProductsMapperService.Mapping_Profiles;
using Zoobee.Core.Errors;
using Zoobee.Domain.DataEntities.Catalog.Tags;
using Zoobee.Domain.DataEntities.Data_Primitives;
using Zoobee.Domain.DataEntities.Environment.Creators;
using Zoobee.Domain.DataEntities.Environment.Manufactures;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Domain.DataEntities.Products.ToiletProductEntity;
using Zoobee.Infrastructure.Services.DtoMappingService;

namespace Zoobee.Test.UnitTests.Infrastructure.Services
{
	[TestFixture]
	public class DtoMappingServiceTest

	{
		private DtoMappingService mappingService;

		Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
		Mock<IEnvirontmentDataUnitOfWork> envDataUnitOfWorkMock = new Mock<IEnvirontmentDataUnitOfWork>();
		Mock<ITagsRepository> tagsRepositoryMock = new Mock<ITagsRepository>();
		Mock<IStringLocalizer<Errors>> localizerMock = new Mock<IStringLocalizer<Errors>>();

		private ToiletProductDto MappableDto = new ToiletProductDto
		{
			Name = "TestProduct",
			Article = "1234567890",
			BrandName = "EcoPet",
			ProductLineupName = "GreenLine",
			Description = "TestDescriptionTestDescriptionTestDescription",
			CreatorCompanyName = "Royal Canin",
			CreatorCountryName = "США",
			EAN = "1231231231232",
			UPC = "123321321123",
			MaxPrice = 15,
			MinPrice = 10,
			MediaURIs = new List<string>(),
			petAgeRange = new PetAgeRange(5, 15),
			SizeDimensions = new SizeDimensions(14, 25, 32),
			PetKind = "Кошки",
			Rating = 152,
			Tags = new List<string>(),
			ToiletType = ToiletType.Diapers,
			VolumeLiters = 3.3f
		};


		[SetUp]
		public void Setup()
		{
			//TODO Замокать зависимости и протестить маппинг0
			envDataUnitOfWorkMock.Setup(
				e => e.ManufacturesUOWork.CreatorCompaniesRepository.Get(MappableDto.CreatorCompanyName))
				.Returns(new CreatorCompanyEntity { CompanyName = MappableDto.CreatorCompanyName });
			envDataUnitOfWorkMock.Setup(
				e => e.ManufacturesUOWork.CreatorCountryRepository.Get(MappableDto.CreatorCountryName))
				.Returns(new CreatorCountryEntity { CountryName = MappableDto.CreatorCountryName });
			envDataUnitOfWorkMock.Setup(
							e => e.ManufacturesUOWork.ProductLineupRepository.Get(MappableDto.BrandName, MappableDto.ProductLineupName))
							.Returns(new ProductLineupEntity { LineupName = MappableDto.ProductLineupName });
			envDataUnitOfWorkMock.Setup(
							e => e.ManufacturesUOWork.BrandsRepository.Get(MappableDto.BrandName))
							.Returns(new BrandEntity { BrandName = MappableDto.BrandName });

			envDataUnitOfWorkMock.Setup(
							e => e.PetsDataUOWork.petKindsRepository.Get(MappableDto.PetKind))
							.Returns(new PetKindEntity { PetKindName = MappableDto.PetKind });
			tagsRepositoryMock.Setup(repo => repo.GetAll())
				.Returns(new List<TagEntity>().AsQueryable());

			localizerMock.Setup(l => l[It.IsAny<string>()])
				.Returns((string name) =>
				{
					return new LocalizedString(name, name, resourceNotFound: true);
				});

			serviceProviderMock.Setup(x => x.GetService(typeof(IBaseMappingProfile<ToiletProductDto, ToiletProductEntity>))).Returns(
				new ToiletProductDtoMappingProfile(localizerMock.Object, envDataUnitOfWorkMock.Object, tagsRepositoryMock.Object));
			mappingService = new DtoMappingService(serviceProviderMock.Object);
		}

		[Test]
		public void TestToiletProductMapping()
		{
			Domain.OperationResult<ToiletProductEntity> resEntity = null;
			try
			{
				resEntity = mappingService.Map<ToiletProductDto, ToiletProductEntity>(MappableDto);
				if (resEntity.Failed)
					Assert.Fail(resEntity.Message);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
			Assert.Pass(resEntity.Returns.Name);
		}
	}
}
