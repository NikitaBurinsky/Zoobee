using NUnit.Framework;
using System.Text.Json;
using System.IO;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Infrastructure.Parsers.Parsers.Zoobazar.Handlers;

namespace Zoobee.Tests.UnitTests.Parsers.Zoobazar
{
	[TestFixture]
	public class ZoobazarFoodParsingTests
	{
		private ZoobazarFoodHandler _handler;

		[SetUp]
		public void Setup()
		{
			_handler = new ZoobazarFoodHandler();
		}

		[TestCase(@"C:\Users\Formatis\Desktop\ZBZ-PRSR\Паштет Perfect Fit для стерилизованных кошек, с индейкой, 75 г купить с доставкой по Минску, Беларуси.html")] // Укажи здесь путь к своему файлу
		public async Task Should_Parse_Html_To_FoodProductDto(string filePath)
		{
			// 1. Проверяем существование файла
			if (!File.Exists(filePath))
			{
				Assert.Ignore($"Файл не найден: {filePath}");
			}

			// 2. Считываем контент
			string htmlContent = await File.ReadAllTextAsync(filePath);

			// 3. Вызываем обработчик (HandleAsync определен в базовом классе)
			// Мы передаем ScrapingTaskType.ProductPage, так как handler проверяет это в CanHandle (если вызываешь через сервис)
			// Но здесь мы вызываем HandleAsync напрямую.
			var result = await _handler.HandleAsync(htmlContent, filePath);

			// 4. Проверяем результат
			Assert.That(result.IsSuccess, $"Парсинг завершился ошибкой: {result.ErrorMessage}");
			Assert.That(result.ExtractedData.ProductInfo != null && result.ExtractedData.ProductSlot != null, "ExtractedData:ProductInfo не должно быть null");
			Assert.That(result.ExtractedData.ProductInfo is FoodProductDto, "Объект должен быть типа FoodProductDto");

			// 5. Сериализуем и выводим в лог
			var options = new JsonSerializerOptions { WriteIndented = true };
			string jsonProduct = JsonSerializer.Serialize((FoodProductDto)result.ExtractedData.ProductInfo, options);
			string jsonSlot = JsonSerializer.Serialize(result.ExtractedData.ProductSlot, options);

			TestContext.WriteLine("--- Parsed FoodProductDto ---");
			TestContext.WriteLine(jsonProduct);
			TestContext.WriteLine("------------------------------");

			TestContext.WriteLine("--- Parsed SellingSlotDto ---");
			TestContext.WriteLine(jsonSlot);
			TestContext.WriteLine("------------------------------");
		}
	}
}