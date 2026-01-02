using HtmlAgilityPack;
using System.Globalization;
using System.Text.RegularExpressions;
using Zoobee.Application.DTOs.Business_Items.Sellings;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Domain.DataEntities.Data_Primitives;
using Zoobee.Domain.DataEntities.Products.FoodProductEntity; // Пространство имен для PetAgeRange (из твоего примера)
															 // Если PetFoodType лежит в другом месте, добавь соответствующий using, например:
															 // using Zoobee.Core.Enums; 

namespace Zoobee.Infrastructure.Parsers.Parsers.Zoobazar.Handlers
{
	public class ZoobazarFoodHandler : BaseZoobazarProductHandler
	{
		protected override bool IsMatch(HtmlDocument doc)
		{
			// Проверяем, содержит ли страница признаки еды (Корм или Лакомства)
			// Проверяем Breadcrumbs или заголовок
			var breadcrumbs = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'bx-breadcrumb')]")?.InnerText ?? "";
			var title = doc.DocumentNode.SelectSingleNode("//h1")?.InnerText ?? "";

			// Собираем весь значимый текст для проверки
			var checkText = (breadcrumbs + " " + title).ToLower();

			return checkText.Contains("корм")
				   || checkText.Contains("лакомств")
				   || checkText.Contains("паштет")
				   || checkText.Contains("консерв");
		}
		protected override BaseProductDto ParseSpecificData(HtmlDocument doc, BaseProductDto baseDto)
		{
			// 1. Создаем FoodProductDto и переносим базовые свойства
			var foodDto = MapBaseToFood(baseDto);

			var root = doc.DocumentNode;
			var name = foodDto.Name.ToLower();

			// Получаем характеристики для анализа (извлекаем текст свойств еще раз или используем данные из baseDto, если бы они там были. 
			// Но так как их там нет в структурированном виде, пройдемся по HTML или используем DTO поля)
			// Для надежности лучше парсить HTML заново для специфичных полей или использовать уже извлеченные текстовые данные.

			// 2. Парсим Вес (ProductWeightGrams)
			foodDto.ProductWeightGrams = ParseWeight(root, name);

			// 3. Парсим Тип Корма (FoodType)
			foodDto.FoodType = ParseFoodType(name, root);

			// 4. Парсим Возраст (PetAgeRange)
			foodDto.PetAgeRange = ParseAgeRange(root);

			return foodDto;
		}

		private FoodProductDto MapBaseToFood(BaseProductDto baseDto)
		{
			return new FoodProductDto
			{
				Id = baseDto.Id,
				Name = baseDto.Name,
				Description = baseDto.Description,
				SiteArticles = baseDto.SiteArticles,
				UPC = baseDto.UPC,
				EAN = baseDto.EAN,
				Rating = baseDto.Rating,
				MinPrice = baseDto.MinPrice,
				MaxPrice = baseDto.MaxPrice,
				CreatorCountryName = baseDto.CreatorCountryName,
				BrandName = baseDto.BrandName,
				ProductLineupName = baseDto.ProductLineupName,
				CreatorCompanyName = baseDto.CreatorCompanyName,
				Tags = baseDto.Tags,
				PetKind = baseDto.PetKind,
				MediaURIs = baseDto.MediaURIs
			};
		}
		private decimal? ParseWeight(HtmlNode root, string productName)
		{
			// 1. Пробуем найти вес в характеристиках (новая структура)
			// Ищем блок с label "Вес"
			var weightNode = root.SelectSingleNode("//div[@class='product-property' and contains(.//div[@class='product-property__label'], 'Вес')]//div[@class='product-property__value']");
			string textToParse = weightNode?.InnerText ?? productName; // Если свойства нет, используем имя товара

			if (string.IsNullOrWhiteSpace(textToParse)) return null;

			// Регулярка ищет числа с "кг", "г", "kg", "g" (например: 1.5 кг, 100г, 15kg, 2 кг)
			var regex = new Regex(@"(\d+(?:[.,]\d+)?)\s*(кг|г|kg|g)\b", RegexOptions.IgnoreCase);
			var match = regex.Match(textToParse);

			if (match.Success)
			{
				var valueString = match.Groups[1].Value.Replace(",", ".");
				var unit = match.Groups[2].Value.ToLower();

				if (decimal.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value))
				{
					if (unit == "кг" || unit == "kg")
					{
						return value * 1000m; // Переводим кг в граммы
					}
					return value; // Уже в граммах
				}
			}
			return null;
		}

		private PetFoodType ParseFoodType(string nameLowerCase, HtmlNode root)
		{
			// 1. Ищем в характеристиках поле "Вид корма"
			var typeNode = root.SelectSingleNode("//div[@class='product-property' and contains(.//div[@class='product-property__label'], 'Вид корма')]//div[@class='product-property__value']");
			var typeText = (typeNode?.InnerText ?? "").ToLower();

			// Если в характеристиках пусто, используем название товара для анализа
			var textToCheck = string.IsNullOrWhiteSpace(typeText) ? nameLowerCase : typeText + " " + nameLowerCase;

			// Приоритеты проверок
			if (textToCheck.Contains("veterinary") || textToCheck.Contains("diet") || textToCheck.Contains("диет") || textToCheck.Contains("лечебн"))
				return PetFoodType.Veterinary;

			if (textToCheck.Contains("holistic") || textToCheck.Contains("холистик"))
				return PetFoodType.Holistic;

			if (textToCheck.Contains("лакомств") || textToCheck.Contains("кость") || textToCheck.Contains("печенье") || textToCheck.Contains("снек"))
				return PetFoodType.Treat;

			if (textToCheck.Contains("влажный") || textToCheck.Contains("паштет") || textToCheck.Contains("пауч") ||
				textToCheck.Contains("консерв") || textToCheck.Contains("желе") || textToCheck.Contains("соус"))
				return PetFoodType.Wet;

			if (textToCheck.Contains("сухой"))
				return PetFoodType.Dry;

			// Fallback
			return PetFoodType.Dry;
		}

		private PetAgeRange? ParseAgeRange(HtmlNode root)
		{
			// Собираем текст из возможных полей: "Возраст", "Показания", "Для кого"
			// Используем XPath для поиска div, у которого label содержит нужные слова
			var nodes = root.SelectNodes("//div[@class='product-property' and (contains(.//div[@class='product-property__label'], 'Возраст') or contains(.//div[@class='product-property__label'], 'Показания') or contains(.//div[@class='product-property__label'], 'Для кого'))]//div[@class='product-property__value']");

			string ageText = "";
			if (nodes != null)
			{
				// Собираем весь текст из найденных свойств (с учетом тегов <a> внутри)
				ageText = string.Join(" ", nodes.Select(n => n.InnerText)).ToLower();
			}

			// Если в свойствах пусто, берем заголовок
			if (string.IsNullOrWhiteSpace(ageText))
			{
				ageText = root.SelectSingleNode("//h1[@id='pagetitle']")?.InnerText.ToLower() ?? "";
			}

			// Логика маппинга (в неделях)
			// 1 год = 52 недели

			if (ageText.Contains("щенк") || ageText.Contains("кот") || ageText.Contains("junior") || ageText.Contains("юниор") || ageText.Contains("дет") || ageText.Contains("starter") || ageText.Contains("беремен"))
			{
				// Junior: 0 - 1.2 года
				return new PetAgeRange(0, 60);
			}

			if (ageText.Contains("пожил") || ageText.Contains("senior") || ageText.Contains("старею") || ageText.Contains("7+"))
			{
				// Senior: 7+ лет
				return new PetAgeRange(364, 1500);
			}

			if (ageText.Contains("взросл") || ageText.Contains("adult"))
			{
				// Adult: 1 - 8 лет
				return new PetAgeRange(52, 400);
			}

			if (ageText.Contains("всех") || ageText.Contains("любого"))
			{
				// All ages
				return new PetAgeRange(0, 1500);
			}

			return null;
		}

	}
}