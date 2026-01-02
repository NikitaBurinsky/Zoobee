using HtmlAgilityPack;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Zoobee.Application.DTOs.Business_Items.Sellings;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Core.Transformation;
using Zoobee.Infrastructure.Parsers.Interfaces.Transformation;

namespace Zoobee.Infrastructure.Parsers.Parsers.Zoobazar.Handlers
{
	public abstract class BaseZoobazarProductHandler : IResourceHandler
	{
		public string TargetSourceName => "Zoobazar";

		public bool CanHandle(ScrapingTaskType taskType, string content)
		{
			if (taskType != ScrapingTaskType.ProductPage) return false;

			var doc = new HtmlDocument();
			doc.LoadHtml(content);
			return IsMatch(doc);
		}

		public Task<TransformationResult> HandleAsync(string content, string url)
		{
			var doc = new HtmlDocument();
			doc.LoadHtml(content);

			var baseDto = ParseCommonData(doc, url); //РЕАЛИЗУЕМ ЭТОТ МЕТОД 

			// Если имя не нашли - считаем ошибкой (возможно, капча или 404)
			if (string.IsNullOrWhiteSpace(baseDto.Name))
			{
				return Task.FromResult(new TransformationResult
				{
					IsSuccess = false,
					ErrorMessage = "Failed to parse product name"
				});
			}

			var finalDto = ParseSpecificData(doc, baseDto);
			var sellingData = ParseSellingSlot(doc, url);

			return Task.FromResult(new TransformationResult
			{
				ExtractedData = new(finalDto, sellingData)
			});
		}

		protected abstract bool IsMatch(HtmlDocument doc);
		protected abstract BaseProductDto ParseSpecificData(HtmlDocument doc, BaseProductDto baseDto);
		protected virtual SellingSlotDto ParseSellingSlot(HtmlDocument doc, string url)
		{
			var dto = new SellingSlotDto();
			var root = doc.DocumentNode;
			//TODO Добавить логгирование в метод
			// "Zoobazar"
			dto.SellerCompanyName = TargetSourceName;
			dto.SellingUrl = url;
			var prices = ParsePrices(doc.Text);
			if (prices.CurrentPrice == null)
				return null;
			dto.ResultPrice = (decimal)prices.CurrentPrice;
			if (prices.OriginalPrice == null)
				dto.DefaultSlotPrice = 0;
			else
				dto.DefaultSlotPrice = (decimal)prices.OriginalPrice;

			if (prices.DiscountPercent == null)
			{
				dto.Discount = dto.ResultPrice / dto.DefaultSlotPrice * 100;
			}
			else
				dto.Discount = (decimal)prices.DiscountPercent;
			return dto;
		}
		// --- МЕТОДЫ ПАРСИНГА ЦЕН ---
		// --- МЕТОДЫ ПАРСИНГА ЦЕН (NULLABLE ВЕРСИЯ) ---

		private (decimal? CurrentPrice, decimal? OriginalPrice, float DiscountPercent) ParsePrices(string content)
		{
			// 1. Текущая цена (result)
			decimal? currentPrice = GetBitrixPriceValue(content, "result");

			// 2. Базовая цена (base) - цена до скидки
			decimal? originalPrice = GetBitrixPriceValue(content, "base");

			// 3. Процент скидки
			float discount = 0;
			var discountMatch = Regex.Match(content, "\"price\".*?\"discountPercent\"\\s*:\\s*(\\d+)", RegexOptions.Singleline);
			if (discountMatch.Success)
			{
				float.TryParse(discountMatch.Groups[1].Value, out discount);
			}

			// ЛОГИКА ОБРАБОТКИ ПУСТОТ:
			// Если мы нашли текущую цену, но не нашли базовую (например, товар без скидки),
			// то считаем, что базовая цена равна текущей.
			if (currentPrice.HasValue && !originalPrice.HasValue)
			{
				originalPrice = currentPrice;
			}

			return (currentPrice, originalPrice, discount);
		}

		// Универсальный метод возвращает decimal? (или null, если не нашел)
		private decimal? GetBitrixPriceValue(string content, string priceKey)
		{
			if (string.IsNullOrEmpty(content)) return null;

			// Регулярка ищет: "price": { ... "ключ": "значение"
			var pattern = $"\"price\".*?\"{priceKey}\"\\s*:\\s*\"(.*?)\"";

			var match = Regex.Match(content, pattern, RegexOptions.Singleline);
			if (match.Success)
			{
				string raw = match.Groups[1].Value;

				try { raw = Regex.Unescape(raw); } catch { }

				// Очистка от всего кроме цифр, запятых и точек
				string clean = Regex.Replace(raw, @"[^\d,.]", "");
				clean = clean.Substring(0, clean.Length - 1);				

				// Нормализация разделителя
				clean = clean.Replace(",", ".");

				if (decimal.TryParse(clean, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
				{
					return price;
				}
			}

			return null; // Явный null, если не спарсилось
		}

		protected BaseProductDto ParseCommonData(HtmlDocument doc, string url)
		{
			var dto = new BaseProductDto
			{
				Id = Guid.NewGuid(),
				SiteArticles = new Dictionary<string, string>(),
				Tags = new List<string>(),
				MediaURIs = new List<string>(),
				MinPrice = 0,
				MaxPrice = 0
			};

			// 1. Основные данные (Приоритет: JSON-LD -> HTML)
			dto.Name = GetName(doc);
			dto.Description = GetDescription(doc);
			dto.Rating = GetRating(doc);

			// 2. Артикул
			var article = GetZoobazarArticle(doc);
			if (!string.IsNullOrEmpty(article))
			{
				dto.SiteArticles.Add("Zoobazar", article);
			}

			// 3. Бренд и Производитель
			dto.BrandName = GetBrand(doc);
			dto.CreatorCompanyName = GetCreatorCompanyName(doc);

			// Если производитель не указан явно, часто это сам бренд
			if (string.IsNullOrWhiteSpace(dto.CreatorCompanyName))
			{
				dto.CreatorCompanyName = dto.BrandName;
			}

			// 4. Характеристики (Парсятся из HTML списка свойств)
			dto.CreatorCountryName = GetCreatorCountryName(doc);
			dto.ProductLineupName = GetProductLineupName(doc);
			dto.PetKind = GetPetKind(doc);

			// 5. Медиа (JSON + Галерея HTML)
			dto.MediaURIs = GetMediaURIs(doc);

			return dto;
		}

		// =========================================================
		// ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ (Private)
		// =========================================================

		// --- 1. Работа с JSON-LD ---

		private JsonDocument GetJsonLdContainer(HtmlDocument doc)
		{
			var scripts = doc.DocumentNode.SelectNodes("//script[@type='application/ld+json']");
			if (scripts == null) return null;

			foreach (var script in scripts)
			{
				try
				{
					var text = script.InnerText;
					var json = JsonDocument.Parse(text);

					if (json.RootElement.TryGetProperty("@type", out var type))
					{
						var typeStr = type.ToString();
						// Ищем именно Product
						if (typeStr.Contains("Product")) return json;
					}
				}
				catch { continue; }
			}
			return null;
		}

		// --- 2. Получение конкретных полей ---

		private string GetName(HtmlDocument doc)
		{
			// 1. JSON-LD
			using var json = GetJsonLdContainer(doc);
			if (json != null && json.RootElement.TryGetProperty("name", out var val))
			{
				return System.Net.WebUtility.HtmlDecode(val.GetString());
			}

			// 2. Fallback HTML
			var node = doc.DocumentNode.SelectSingleNode("//h1[@id='pagetitle']")
					   ?? doc.DocumentNode.SelectSingleNode("//h1");
			return node != null ? System.Net.WebUtility.HtmlDecode(node.InnerText.Trim()) : string.Empty;
		}

		private string GetBrand(HtmlDocument doc)
		{
			// 1. JSON-LD
			using var json = GetJsonLdContainer(doc);
			if (json != null && json.RootElement.TryGetProperty("brand", out var brandObj))
			{
				if (brandObj.ValueKind == JsonValueKind.Object && brandObj.TryGetProperty("name", out var brandName))
					return System.Net.WebUtility.HtmlDecode(brandName.GetString());
				else if (brandObj.ValueKind == JsonValueKind.String)
					return System.Net.WebUtility.HtmlDecode(brandObj.GetString());
			}

			// 2. Fallback HTML
			return GetPropertyValue(doc, "Бренд") ?? GetPropertyValue(doc, "Производитель");
		}

		private string GetDescription(HtmlDocument doc)
		{
			// Приоритет: <meta itemprop="description" content="...">
			var metaNode = doc.DocumentNode.SelectSingleNode("//meta[@itemprop='description']");
			if (metaNode != null)
			{
				var content = metaNode.GetAttributeValue("content", "");
				if (!string.IsNullOrWhiteSpace(content))
				{
					return System.Net.WebUtility.HtmlDecode(content.Trim());
				}
			}

			// Fallback: Если мета-тега нет, пробуем старые способы (на всякий случай)
			var node = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'product-card__description')]")
					   ?? doc.DocumentNode.SelectSingleNode("//*[contains(@class, 'detail_text')]");

			return node != null ? System.Net.WebUtility.HtmlDecode(node.InnerText.Trim()) : string.Empty;
		}
		private float GetRating(HtmlDocument doc)
		{
			// 1. JSON-LD (Самый надежный)
			using var json = GetJsonLdContainer(doc);
			if (json != null && json.RootElement.TryGetProperty("aggregateRating", out var ratingObj))
			{
				if (ratingObj.TryGetProperty("ratingValue", out var val))
				{
					string valStr = val.ToString();
					if (float.TryParse(valStr.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
						return result;
				}
			}

			// 2. Fallback HTML
			var node = doc.DocumentNode.SelectSingleNode("//*[contains(@class, 'rating')]//*[contains(@class, 'value')]");
			if (node != null && float.TryParse(node.InnerText.Replace(".", ","), out float htmlRating))
			{
				return htmlRating;
			}

			return 0f;
		}

		private string GetZoobazarArticle(HtmlDocument doc)
		{
			// 1. JSON-LD
			using var json = GetJsonLdContainer(doc);
			if (json != null && json.RootElement.TryGetProperty("sku", out var val))
			{
				return val.GetString();
			}

			// 2. Fallback HTML
			// Пробуем найти через универсальный метод свойства
			var fromProp = GetPropertyValue(doc, "Артикул");
			if (!string.IsNullOrEmpty(fromProp)) return fromProp;

			// Пробуем старый метод поиска по классу
			var node = doc.DocumentNode.SelectSingleNode("//*[contains(@class, 'article')]//span[@class='value']");
			return node != null ? node.InnerText.Trim() : string.Empty;
		}

		// --- 3. Характеристики (Обертки над GetPropertyValue) ---

		// --- ВСПОМОГАТЕЛЬНЫЙ МЕТОД (Парсит JSON Bitrix из текста) ---
		private string GetBitrixProperty(HtmlDocument doc, string propertyCode)
		{
			// Берем полный исходный текст страницы (включая скрипты)
			string content = doc.ParsedText;
			if (string.IsNullOrEmpty(content)) return null;

			// Регулярка ищет структуру: "КОД_СВОЙСТВА": { ... "VALUE": "Значение" ... }
			// Используем Singleline, чтобы искать даже если JSON разбит на строки
			var pattern = $"\"{propertyCode}\"\\s*:\\s*\\{{.*?\"VALUE\"\\s*:\\s*\"(.*?)\"";

			var match = Regex.Match(content, pattern, RegexOptions.Singleline);

			if (match.Success)
			{
				// Получаем сырое значение
				var rawValue = match.Groups[1].Value;

				// В JSON слэши часто экранированы (Франция\/Россия -> Франция/Россия)
				// Regex.Unescape также превратит \uXXXX в нормальные буквы
				try
				{
					// Сначала убираем экранирование слэшей, которое Unescape может не понять
					rawValue = rawValue.Replace("\\/", "/");
					return Regex.Unescape(rawValue).Trim();
				}
				catch
				{
					return rawValue.Trim();
				}
			}

			return null;
		}

		// --- 1. СТРАНА ПРОИСХОЖДЕНИЯ ---
		private string GetCreatorCountryName(HtmlDocument doc)
		{
			// Код свойства из твоего примера: STRANA_PROISKHOZHDENIYA
			return GetBitrixProperty(doc, "STRANA_PROISKHOZHDENIYA") ?? string.Empty;
		}

		// --- 2. КОМПАНИЯ ПРОИЗВОДИТЕЛЬ ---
		private string GetCreatorCompanyName(HtmlDocument doc)
		{
			// Ты нашел "ADRES_PROIZVODITELYA" - там лежит название юр.лица ("АО РУСКАН...")
			// На всякий случай проверяем и просто "PROIZVODITEL"
			return GetBitrixProperty(doc, "PROIZVODITEL")
				   ?? GetBitrixProperty(doc, "ADRES_PROIZVODITELYA")
				   ?? string.Empty;
		}

		// --- 3. ЛИНЕЙКА БРЕНДА ---
		private string GetProductLineupName(HtmlDocument doc)
		{
			// Стандартные коды свойств Bitrix для линейки
			return GetBitrixProperty(doc, "LINEYKA_BRENDA")
				   ?? GetBitrixProperty(doc, "LINEYKA")
				   ?? string.Empty;
		}

		// --- 4. ТИП ПИТОМЦА ---
		private string GetPetKind(HtmlDocument doc)
		{
			string content = doc.ParsedText;
			if (string.IsNullOrEmpty(content)) return string.Empty;

			// Регулярка работает так:
			// 1. Ищет "TIP_PITOMTSA" (или VID_ZHIVOTNOGO)
			// 2. Пропускает всё до ключа "VALUE"
			// 3. Жестко ищет открытие массива '[' и кавычку '"'
			// 4. Забирает всё до следующей кавычки

			var pattern = "\"(?:TIP_PITOMTSA|VID_ZHIVOTNOGO)\".*?\"VALUE\"\\s*:\\s*\\[\\s*\"(.*?)\"";

			var match = Regex.Match(content, pattern, RegexOptions.Singleline);

			if (match.Success)
			{
				string raw = match.Groups[1].Value;

				// Чистим от Unicode (\uXXXX) и экранирования
				try
				{
					return Regex.Unescape(raw.Replace("\\/", "/")).Trim();
				}
				catch
				{
					return raw.Trim();
				}
			}

			return string.Empty;
		}
		// --- 4. Универсальный парсер свойств (С учетом сырой верстки из TEST.txt) ---
		private string GetPropertyValue(HtmlDocument doc, string labelText)
		{
			// --- ВАРИАНТ 1: Твоя новая схема (из браузера) ---
			// <div class="product-property">
			//    <div class="product-property__label">Производитель</div> 
			//    <div class="product-property__value">Значение</div>
			// </div>

			// Ищем div-label с нужным текстом
			var divLabel = doc.DocumentNode.SelectSingleNode($"//div[contains(@class, 'product-property__label') and contains(text(), '{labelText}')]");

			if (divLabel != null)
			{
				// Ищем соседний div с классом value
				var valueDiv = divLabel.ParentNode.SelectSingleNode(".//div[contains(@class, 'product-property__value')]");
				// Или просто следующий сосед, если структура плоская
				if (valueDiv == null)
					valueDiv = divLabel.SelectSingleNode("following-sibling::div[contains(@class, 'product-property__value')]");

				if (valueDiv != null)
				{
					return System.Net.WebUtility.HtmlDecode(valueDiv.InnerText.Trim());
				}
			}

			// --- ВАРИАНТ 2: Схема сырого HTML (из TEST.txt) ---
			// <li> <span>Производитель</span> <b>Значение</b> </li>

			var liSpan = doc.DocumentNode.SelectSingleNode($"//li/span[contains(text(), '{labelText}')]");
			if (liSpan != null)
			{
				var parentLi = liSpan.ParentNode;
				var valueNode = parentLi.SelectSingleNode(".//b") ?? parentLi.SelectSingleNode(".//a");

				if (valueNode != null)
				{
					return System.Net.WebUtility.HtmlDecode(valueNode.InnerText.Trim());
				}

				// Если тегов нет, берем текст родителя без метки
				var fullText = parentLi.InnerText;
				var cleanValue = fullText.Replace(liSpan.InnerText, "").Replace(":", "").Trim();
				return System.Net.WebUtility.HtmlDecode(cleanValue);
			}

			return string.Empty;
		}
		// --- 5. Изображения ---

		private List<string> GetMediaURIs(HtmlDocument doc)
		{
			var urls = new List<string>();
			using var json = GetJsonLdContainer(doc);

			// Главное фото из JSON
			if (json != null && json.RootElement.TryGetProperty("image", out var imgProp))
			{
				var imgUrl = imgProp.GetString();
				if (!string.IsNullOrWhiteSpace(imgUrl)) urls.Add(EnsureAbsoluteUrl(imgUrl));
			}

			// Галерея из HTML (сырой код)
			var nodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'slider') or contains(@class, 'gallery')]//img");
			if (nodes != null)
			{
				foreach (var node in nodes)
				{
					// Часто src заглушка, реальное фото в data-src
					var src = node.GetAttributeValue("data-src", node.GetAttributeValue("src", ""));

					if (!string.IsNullOrWhiteSpace(src) && !src.Contains("gif"))
					{
						var fullUrl = EnsureAbsoluteUrl(src);
						if (!urls.Contains(fullUrl)) urls.Add(fullUrl);
					}
				}
			}
			return urls;
		}

		private string EnsureAbsoluteUrl(string url)
		{
			if (url.StartsWith("http")) return url;
			if (url.StartsWith("/")) return "https://zoobazar.by" + url;
			return "https://zoobazar.by/" + url.TrimStart('/');
		}
	}
}