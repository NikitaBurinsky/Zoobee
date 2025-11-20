using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Text.Json;
using Zoobee.Infrastructure.Parsers.Storage.ParsedDataEntities.ParsedProductsModels.Zoobazar;

namespace Zoobee.Infrastructure.Parsers.Pipelines.Scraping.Parsers.Zoobazar.Client.Tests
{
	[TestFixture]
	public class ZoobazarClientTests
	{
		private ILogger<ZoobazarClient> logger;
		private HttpClient _httpClient = new();
		private Mock<IConfiguration> _configurationMock;
		private ZoobazarClient _client;

		[SetUp]
		public void SetUp()
		{
			var loggerFactory = LoggerFactory.Create(builder =>
			{
				builder
					.AddConsole()
					.SetMinimumLevel(LogLevel.Debug);
			});
			logger = loggerFactory.CreateLogger<ZoobazarClient>();
			
			_configurationMock = new Mock<IConfiguration>();
			_client = new ZoobazarClient(logger, _httpClient, _configurationMock.Object);
		}

		[Test]
		public async Task TestParse()
		{
			var html = await _httpClient.GetStringAsync
				(@"https://zoobazar.by/catalog/sobaki/sukhie_korma/premium_klassa_5/korm_sirius_dlya_vzroslykh_sobak_s_govyadinoy_i_ovoshchami_15_kg/");
			var res = ZoobazarClient.ExtractProductCardJson(html);
			if (res == null)
				Assert.Fail("Не найдена продуктовая-карта.");
			
			Assert.That(HasNonEmptySale(res));
		}

		public bool HasNonEmptySale(string content)
		{
			// Ищем паттерн "sale":[] (пустой массив)
			if (content.Contains("\"sale\":[]"))
			{
				return false;
			}

			// Ищем паттерн "sale":[ с любым содержимым кроме пустого массива
			int saleIndex = content.IndexOf("\"sale\":[");
			if (saleIndex == -1)
				return false; // поле sale не найдено

			// Находим закрывающую скобку для этого массива
			int bracketCount = 1;
			int currentIndex = saleIndex + "\"sale\":[".Length;

			while (currentIndex < content.Length && bracketCount > 0)
			{
				if (content[currentIndex] == '[')
					bracketCount++;
				else if (content[currentIndex] == ']')
					bracketCount--;

				currentIndex++;
			}

			// Если между [ и ] есть символы кроме пробельных - массив не пустой
			string arrayContent = content.Substring(saleIndex + "\"sale\":[".Length,
												  currentIndex - (saleIndex + "\"sale\":[".Length) - 1);

			return !string.IsNullOrWhiteSpace(arrayContent);
		}
	}
}