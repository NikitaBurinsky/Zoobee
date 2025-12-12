using HtmlAgilityPack;
using System.Threading.Tasks;
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

			var baseDto = ParseCommonData(doc);

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

			return Task.FromResult(new TransformationResult
			{
				ExtractedData = finalDto
			});
		}

		protected abstract bool IsMatch(HtmlDocument doc);
		protected abstract object ParseSpecificData(HtmlDocument doc, BaseProductDto baseDto);

		private BaseProductDto ParseCommonData(HtmlDocument doc)
		{
			// Здесь будет реальная логика парсинга Zoobazar (Название, Цена, Картинка)
			return new BaseProductDto
			{
				Name = doc.DocumentNode.SelectSingleNode("//h1")?.InnerText?.Trim()
				// Price = ...
			};
		}
	}
}