using HtmlAgilityPack;
using System.Globalization;
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

			var baseDto = ParseCommonData(doc, url);



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
		protected abstract SellingSlotDto ParseSellingSlot(HtmlDocument doc, string url);

		private BaseProductDto ParseCommonData(HtmlDocument doc, string url)
		{
			var dto = new BaseProductDto
			{
				MediaURIs = new List<string>(),
				Tags = new List<string>(),
				SiteArticles = new Dictionary<string, string>()
			};

			var root = doc.DocumentNode;

			var nameNode = root.SelectSingleNode("//h1[@id='pagetitle']");
			dto.Name = nameNode?.InnerText.Trim() ?? "Unknown Name";

			var descNode = root.SelectSingleNode("//div[contains(@class, 'product-card__description')]");
			dto.Description = descNode?.InnerText.Trim();

			// 3. Рейтинг (Rating) - Берем из <span class="value">4.9</span> (внутри родительского блока)
			// Используем более общий, но целевой поиск, так как контекст может быть разным.
			var ratingNode = root.SelectSingleNode("//div[contains(@class, 'product-card__rating')]//span[contains(@class, 'value')]");
			if (ratingNode == null)
			{
				// Альтернативный поиск по старой микроразметке (более надежно, если она есть)
				ratingNode = root.SelectSingleNode("//meta[@itemprop='ratingValue']");
			}

			if (ratingNode != null)
			{
				// Если это span.value, берем InnerText, иначе берем 'content' из meta
				var ratingText = ratingNode.GetAttributeValue("content", ratingNode.InnerText ?? "0").Replace(",", ".");
				if (float.TryParse(ratingText, NumberStyles.Any, CultureInfo.InvariantCulture, out float rating))
				{
					dto.Rating = rating;
				}
			}

			// 4. Изображения (MediaURIs)
			// Ищем все <img> внутри основного контейнера изображений
			var imageNodes = root.SelectNodes("//div[contains(@class, 'product-card__media')]//img");
			if (imageNodes != null)
			{
				foreach (var img in imageNodes)
				{
					// Берем ссылку из srcset, если есть (для лучшего качества), иначе из src
					var src = img.GetAttributeValue("srcset", null) ?? img.GetAttributeValue("src", null);

					if (!string.IsNullOrEmpty(src))
					{
						// Если srcset содержит несколько ссылок, берем первую (обычно самую большую)
						var bestSrc = src.Split(',').Last().Trim().Split(' ').First().Trim();

						// Приводим к абсолютному пути, если он относительный
						if (bestSrc.StartsWith("/")) bestSrc = "https://zoobazar.by" + bestSrc;

						if (!dto.MediaURIs.Contains(bestSrc))
							dto.MediaURIs.Add(bestSrc);
					}
				}
			}

			// 5. Характеристики (Article, Country, Brand, Lineup, Company, PetKind, UPC/EAN)
			ParseAttributes(root, dto);

			return dto;
		}


		// Вспомогательный метод для разбора характеристик по новой структуре
		private void ParseAttributes(HtmlNode root, BaseProductDto dto)
		{
			// Ищем все элементы-свойства: <div class="product-property">
			var propNodes = root.SelectNodes("//div[@class='product-property']");

			if (propNodes == null) return;

			foreach (var node in propNodes)
			{
				// Извлекаем Название свойства (label)
				var titleNode = node.SelectSingleNode("./div[@class='product-property__label']");

				// Извлекаем Значение свойства (value), очищая текст от возможных тегов <a>
				var valueNode = node.SelectSingleNode("./div[@class='product-property__value']");

				if (titleNode == null || valueNode == null) continue;

				// Нормализуем заголовок (label)
				var title = titleNode.InnerText.Trim().Trim(':');

				// Нормализуем значение (value). Если внутри есть ссылка, берем InnerText контейнера.
				var value = valueNode.InnerText.Trim();

				// 6. Сопоставляем со свойствами DTO
				if (title.Contains("Артикул", StringComparison.OrdinalIgnoreCase) || title.Contains("Код товара", StringComparison.OrdinalIgnoreCase))
				{
					// Артикул магазина добавляем в словарь SiteArticles
					dto.SiteArticles.TryAdd(TargetSourceName, value);
				}
				else if (title.Contains("Страна", StringComparison.OrdinalIgnoreCase) && (title.Contains("происхождения", StringComparison.OrdinalIgnoreCase) || title.Contains("производителя", StringComparison.OrdinalIgnoreCase)))
				{
					dto.CreatorCountryName = value;
				}
				else if (title.Contains("Бренд", StringComparison.OrdinalIgnoreCase))
				{
					dto.BrandName = value;
				}
				else if (title.Contains("Линейка бренда", StringComparison.OrdinalIgnoreCase) || title.Contains("Серия", StringComparison.OrdinalIgnoreCase))
				{
					dto.ProductLineupName = value;
				}
				else if (title.Contains("Производитель", StringComparison.OrdinalIgnoreCase) || title.Contains("Компания", StringComparison.OrdinalIgnoreCase))
				{
					dto.CreatorCompanyName = value;
				}
				else if (title.Contains("Тип питомца", StringComparison.OrdinalIgnoreCase) || title.Contains("Животное", StringComparison.OrdinalIgnoreCase))
				{
					dto.PetKind = value;
				}
				else if (title.Contains("Штрихкод", StringComparison.OrdinalIgnoreCase) || title.Contains("EAN", StringComparison.OrdinalIgnoreCase) || title.Contains("UPC", StringComparison.OrdinalIgnoreCase))
				{
					// UPC и EAN могут быть в одном поле
					dto.EAN = value;
					dto.UPC = value;
				}
			}
		}


	}
}