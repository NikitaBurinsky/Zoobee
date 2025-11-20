using Moq;
using NUnit.Framework;
using Zoobee.Domain.DataEntities.Products.ToiletProductEntity;
using Zoobee.Infrastructure.Repositoties.Products;
using Zoobee.Infrastructure.Repositoties.UnitsOfWork;

namespace ZooStores.Test.Tests.Services.IDtoMappingService
{
	[TestFixture]
	public class ProductsUnitOfWorkTest
	{
		ProductsUnitOfWork productsUnitOfWork;
		Mock<BaseProductsRepository> productsRepositoryMock = new Mock<BaseProductsRepository>(null);
		Mock<FoodProductRepository> foodProductsRepoMock = new Mock<FoodProductRepository>(null, null);
		Mock<ToiletProductRepository> toiletProductsRepoMock = new Mock<ToiletProductRepository>(null, null);

		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void TestToiletProductMapping()
		{
			var uow = new ProductsUnitOfWork(foodProductsRepoMock.Object, toiletProductsRepoMock.Object, productsRepositoryMock.Object);
			var toiletRepo = uow.RepositoryOfType<ToiletProductEntity>();
			Assert.That(toiletRepo is ToiletProductRepository);
		}
	}
}
