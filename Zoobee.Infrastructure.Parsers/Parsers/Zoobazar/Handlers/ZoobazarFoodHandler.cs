using HtmlAgilityPack;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.DTOs.Products.Types;

namespace Zoobee.Infrastructure.Parsers.Parsers.Zoobazar.Handlers
{/*
	public class ZoobazarFoodHandler : BaseZoobazarProductHandler
	{
		protected override bool IsMatch(HtmlDocument doc)
		{
			// Ищем слово "Корм" в хлебных крошках или характеристиках
			// Пример: <ul class="breadcrumbs">...<li>Корм</li>...</ul>
			var text = doc.DocumentNode.InnerText;
			return text.Contains("Корм") || text.Contains("Лакомства");
		}

		protected override object ParseSpecificData(HtmlDocument doc, BaseProductDto baseDto)
		{
			// Маппим Base -> Food
			var food = new FoodProductDto
			{
				Id = baseDto.Id,
				Name = baseDto.Name,
				// ... копируем остальное
				Ingredients = "Курица, Рис..." // Тут будет XPath для состава
			};
			return food;
		}
	}*/
}