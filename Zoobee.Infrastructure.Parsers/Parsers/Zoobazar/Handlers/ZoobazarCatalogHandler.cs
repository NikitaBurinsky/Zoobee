using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Core.Transformation;
using Zoobee.Infrastructure.Parsers.Interfaces.Transformation;
using Zoobee.Infrastructure.Parsers.Parsers.Zoobazar.Services;

namespace Zoobee.Infrastructure.Parsers.Parsers.Zoobazar.Handlers
{
	public class ZoobazarCatalogHandler : IResourceHandler
	{
		public string TargetSourceName => "Zoobazar";

		ILogger<ZoobazarCatalogHandler> logger;

		public ZoobazarCatalogHandler(ILogger<ZoobazarCatalogHandler> logger)
		{
			this.logger = logger;
		}
		//TODO Переписать CanHandle?, поскольку все товары в url тоже содержат c atalog, требуется уточнить
		public bool CanHandle(ScrapingTaskType taskType, string content, string url)
		{
			if (taskType == ScrapingTaskType.ListingPage) return true;
			bool catalog_main = content.Contains("<div class=\"catalog__main\">");
			bool catalog_head = content.Contains("<div class=\"catalog__heading\">");

			return catalog_head && catalog_main;
		}

		public async Task<TransformationResult> HandleAsync(string content, string url)
		{
			logger.LogInformation("Начат процесс трансформации в хендлере. Url: {Url}", url);
			var transformResult = new TransformationResult
			{
				IsSuccess = true,
				ExtractedData = (null, null),
				NewTasks = new List<(string Url, ScrapingTaskType Type)>()
			};
			var links = await ZoobazarLinksExtractor.ExtractLinksAsync(content, "https://zoobazar.by");
			foreach (var link in links)
			{
				if (link.Contains("://zoobazar"))
				{
					transformResult.NewTasks.Add(new(link, ScrapingTaskType.Unknown));
				}
			}
			var match = Regex.Match(url, @"PAGEN_2=(\d+)", RegexOptions.IgnoreCase);
			int page = match.Success ? int.Parse(match.Groups[1].Value) : 1;
			int newPageNumber = ++page;

			string nextCatalogLink;
			if (match.Success)
				nextCatalogLink = Regex.Replace(
									url,
									@"(PAGEN_2=)\d+",
									$"$1{newPageNumber}",
									RegexOptions.IgnoreCase
								);
			else
				nextCatalogLink = Path.Combine(url, $"PAGEN_2={newPageNumber}");

				transformResult.NewTasks.Add(new(nextCatalogLink, ScrapingTaskType.ListingPage));
			logger.LogInformation("Обработка {Url} завершена.", url);
			return transformResult;
		}
	}
}
