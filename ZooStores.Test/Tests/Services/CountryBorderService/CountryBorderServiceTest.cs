using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Text.Json;
using Zoobee.Domain.DataEntities.Environment.Geography;
using Zoobee.Infrastructure.Services.GeoServices.CountryBordersService;

namespace ZooStores.Test.Tests.Services.CountryBorderServiceTest
{
	[TestFixture]
	public class CountryBorderServiceTest
	{
		private CountryBorderService CountryBorderService { get; set; }
		private Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
		private Mock<IConfigurationSection> mockConfigBYBordersSection = new Mock<IConfigurationSection>();

		[SetUp]
		public void Setup()
		{
			var border = new List<GeoPoint>()
			{
				new GeoPoint(1,1),
				new GeoPoint(10,1),
				new GeoPoint(10,10),
				new GeoPoint(1,10),
			};/*
			mockConfigBYBordersSection.Setup(x => x.Get<List<GeoPoint>>())
				.Returns(border);
			mockConfiguration.Setup(x => x.GetSection("CountryBorders:BY"))
				.Returns(mockConfigBYBordersSection.Object);*/
			CountryBorderService = new CountryBorderService(mockConfiguration.Object);
		}

		[Test]
		public void CheckBordersValidation1()
		{

			var area1 = new List<GeoPoint>
			{
				new GeoPoint(5,3),
				new GeoPoint(3,5),
				new GeoPoint(3,3),
			};
			var area2 = new List<GeoPoint>
			{
				new GeoPoint(2,12),
				new GeoPoint(2.5,14),
				new GeoPoint(-2,14),
			};
			var area3 = new List<GeoPoint>
			{
				new GeoPoint(-2,2),
				new GeoPoint(2,4),
				new GeoPoint(-4,5),
			};
			var area4 = new List<GeoPoint>
			{
				new GeoPoint(9,2),
				new GeoPoint(9,3),
				new GeoPoint(8,3),
				new GeoPoint(8,2),
			};
			// Act
			var result1 = CountryBorderService.IsPolygonInCountry(area1);
			var result2 = CountryBorderService.IsPolygonInCountry(area2);
			var result3 = CountryBorderService.IsPolygonInCountry(area3);
			var result4 = CountryBorderService.IsPolygonInCountry(area4);

			// Assert
			Assert.That(result1, Is.EqualTo(true), "(1) Failed", "(1) Passed");
			Assert.That(result2, Is.EqualTo(false), "(2) Failed", "(2) Passed");
			Assert.That(result3, Is.EqualTo(false), "(3) Failed", "(3) Passed");
			Assert.That(result4, Is.EqualTo(true), "(4) Failed", "(4) Passed");

		}



	}
}
